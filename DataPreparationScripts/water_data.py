import pandas as pd

# Step 1: Load the dataset
df = pd.read_csv(
    '/Users/sayurugunawardana/Documents/archive/water_consumption.csv',  # Update path as needed
    usecols=['timestamp', 'campus_id', 'meter_id', 'consumption']
)

# Step 2: Parse timestamp (no timezone applied)
df['timestamp'] = pd.to_datetime(df['timestamp'], errors='coerce')
df = df.dropna(subset=['timestamp'])

# Step 3: Filter for 2021 and meter_id == 1
df = df[(df['timestamp'].dt.year == 2021) & (df['meter_id'] == 1)]

# Step 4: Filter for dates starting from 2021-01-10 00:00:00
df = df[df['timestamp'] >= pd.Timestamp("2021-01-10 00:00:00")]

# Step 5: Remove unnecessary columns
df = df.drop(columns=['campus_id', 'meter_id'])

# Step 6: Drop duplicates just in case
df = df.drop_duplicates(subset='timestamp')

# Step 7: Create full 15-min index range
full_index = pd.date_range(start=df['timestamp'].min(), end=df['timestamp'].max(), freq='15min')

# Step 8: Reindex and interpolate
df = df.set_index('timestamp').reindex(full_index)
df['consumption'] = df['consumption'].interpolate(method='linear', limit_direction='both')

# Step 9: Finalize output
df = df.reset_index().rename(columns={'index': 'timestamp'})

# Step 10: Export to CSV
df.to_csv("water_consumption_15min_realistic_filled.csv", index=False)
print("✅ File saved as water_consumption_15min_realistic_filled.csv")