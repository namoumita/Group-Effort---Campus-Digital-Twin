# Data Preparation Scripts

This folder contains the Python scripts used to preprocess and standardize utility consumption datasets into consistent 15-minute intervals. These scripts were used to prepare the input `.csv` files for streaming via Kafka and storing in QuestDB.

## Scripts

### `electricity_data.py`
- Input: 5-minute interval electricity readings
- Process:
  - Filtered data for `building_id == 62` and year `2021`
  - Averaged every 3 rows into 15-minute blocks
  - Filled missing 15-min timestamps using reindexing and interpolation
- Output: `electricity.csv`

### `gas_data.py`
- Input: Hourly gas consumption data
- Process:
  - Filtered data for `campus_id == 1` from `2021-01-02`
  - Expanded each 1-hour value into 4 equal 15-minute intervals
  - Handled missing hours using linear interpolation
- Output: `gas.csv`

### `water_data.py`
- Input: 15-minute water meter data
- Process:
  - Filtered for `meter_id == 1` starting from `2021-01-10`
  - Reindexed to ensure every 15-min interval exists
  - Applied linear interpolation for missing values
- Output: `water.csv`

---

These scripts ensure that all utility datasets are aligned on the same time granularity for unified time-series analysis and streaming pipelines.
