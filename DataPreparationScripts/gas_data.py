import pandas as pd

# Step 1: Load the dataset
df = pd.read_csv(
    '/Users/sayurugunawardana/Documents/archive/gas_consumption.csv',
    usecols=['timestamp', 'campus_id', 'consumption']
)

# Step 2: Parse timestamp as-is (no timezone)
df['timestamp'] = pd.to_datetime(df['timestamp'], errors='coerce')
df = df.dropna(subset=['timestamp'])

# Step 3: Filter for 2021 and campus_id == 1
df = df[(df['timestamp'].dt.year == 2021) & (df['campus_id'] == 1)]

# Step 4: Drop 'campus_id'
df = df.drop(columns=['campus_id'])

# Step 5: Floor all timestamps to nearest hour
df['timestamp'] = df['timestamp'].dt.floor('H')

# Step 6: Filter from 2021-01-02 00:00:00
df = df[df['timestamp'] >= pd.Timestamp("2021-01-02 00:00:00")]

# Step 7: Create full hourly index from first to last timestamp
full_index = pd.date_range(start=df['timestamp'].min(), end=df['timestamp'].max(), freq='H')
df = df.set_index('timestamp').reindex(full_index)

# Step 8: Interpolate missing hourly values
df['consumption'] = df['consumption'].interpolate(method='linear', limit_direction='both')
df = df.reset_index().rename(columns={'index': 'timestamp'})

# Step 9: Expand each 1-hour row to 4 × 15-min intervals
expanded_rows = []
for _, row in df.iterrows():
    base_time = row['timestamp']
    for offset in [0, 15, 30, 45]:
        expanded_rows.append({
            'timestamp': base_time + pd.Timedelta(minutes=offset),
            'consumption': row['consumption']
        })

# Step 10: Create final DataFrame
expanded_df = pd.DataFrame(expanded_rows)
expanded_df = expanded_df.sort_values(by='timestamp').reset_index(drop=True)

# Step 11: Export to CSV
expanded_df.to_csv("gas_consumption_15min_realistic_filled.csv", index=False)
print("✅ Saved: gas_consumption_15min_realistic_filled.csv")