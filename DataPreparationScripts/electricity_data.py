import pandas as pd

# Load and prepare
df = pd.read_csv(
    '/Users/sayurugunawardana/Documents/archive/Campus id_1/building_submeter_consumption.csv',
    usecols=['building_id', 'id', 'timestamp', 'consumption'],
    encoding='ISO-8859-1'
)
df['timestamp'] = pd.to_datetime(df['timestamp'], errors='coerce', utc=True)
df = df.dropna(subset=['timestamp'])
df['timestamp'] = df['timestamp'].dt.tz_convert('Australia/Sydney').dt.tz_localize(None)

# Filter for building_id 62, year 2021
df = df[(df['building_id'] == 62) & (df['timestamp'].dt.year == 2021)]

# Set timestamp index
df = df.sort_values(['id', 'timestamp']).set_index('timestamp')

# Prepare final results list
results = []

# Get all IDs and all days
all_ids = df['id'].unique()
all_days = pd.date_range(start='2021-01-01', end='2021-12-31', freq='D')
daily_time_grid = pd.date_range("00:15", "23:45", freq="15min").time

# Loop by ID
for meter_id in all_ids:
    meter_data = df[df['id'] == meter_id]['consumption']

    # Loop by day
    for day in all_days:
        # Full 15-minute timestamp range for the day
        full_range = pd.date_range(f"{day.date()} 00:15", f"{day.date()} 23:45", freq='15min')

        # Slice actual data for the day
        meter_day_data = meter_data[meter_data.index.date == day.date()]

        # If there's data, resample and interpolate
        if not meter_day_data.empty:
            resampled = meter_day_data.resample("15min").mean().reindex(full_range)
            interpolated = resampled.interpolate(limit_direction='both')
        else:
            # If no data for this day, fill with all NaN
            interpolated = pd.Series(index=full_range, data=[float('nan')] * len(full_range))

        # Assemble day DataFrame
        day_df = pd.DataFrame({
            'building_id': 62,
            'id': meter_id,
            'timestamp': full_range,
            'consumption': interpolated.values
        })
        results.append(day_df)

# Combine all into final output
final_df = pd.concat(results).reset_index(drop=True)

# Save
output_path = "/Users/sayurugunawardana/Desktop/building62_2021_filled_interpolated.csv"
final_df.to_csv(output_path, index=False)
print("✅ File saved with realistic 15-min interpolated values:", output_path)