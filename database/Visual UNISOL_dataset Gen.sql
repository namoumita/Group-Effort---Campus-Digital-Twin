---graph1
SELECT campuskey, SUM(solargeneration) AS total_solar_generation
FROM solar_energy_generation
GROUP BY campuskey
ORDER BY total_solar_generation DESC;


---graph2
SELECT campuskey,
       date_trunc('month', "timestamp") AS month,
       SUM(solargeneration) AS monthly_generation
FROM solar_energy_generation
GROUP BY campuskey, month
ORDER BY month, campuskey;

---heatmap and bubble plot
SELECT
    ssd.campuskey,
    ssd.sitekey,
    ssd.latitude,
    ssd.longitude,
    date_trunc('month', seg."timestamp") AS month,
    SUM(seg.solargeneration) AS total_generation_kwh
FROM
    solar_site_details ssd
JOIN
    solar_energy_generation seg
ON
    ssd.campuskey = seg.campuskey AND ssd.sitekey = seg.sitekey
GROUP BY
    ssd.campuskey, ssd.sitekey, ssd.latitude, ssd.longitude, month
ORDER BY
    month, total_generation_kwh DESC;

---efficiency and weather correlation
SELECT
    s.campuskey,
    s.sitekey,
    DATE_TRUNC('day', s.timestamp) AS date,
    SUM(s.solargeneration) AS total_generation_kwh,
    MAX(d.kWp) AS installed_capacity_kw,
    SUM(s.solargeneration) / NULLIF(MAX(d.kWp), 0) AS efficiency_ratio,
    AVG(w.airtemperature) AS avg_air_temp,
    AVG(w.dewpointtemperature) AS avg_dew_point,
    AVG(w.relativehumidity) AS avg_humidity,
    AVG(w.windspeed) AS avg_wind_speed,
    AVG(w.winddirection) AS avg_wind_direction
FROM solar_energy_generation s
JOIN solar_site_details d
    ON s.sitekey = d.sitekey AND s.campuskey = d.campuskey
JOIN weather_data_recorded_all w
    ON s.campuskey = w.campuskey AND s.timestamp = w.timestamp
GROUP BY s.campuskey, s.sitekey, DATE_TRUNC('day', s.timestamp)
ORDER BY s.campuskey, s.sitekey, date;
