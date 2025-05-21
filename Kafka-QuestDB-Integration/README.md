# Kafka to QuestDB Integration

This module simulates real-time utility data streaming using Kafka and Kafka Connect to ingest electricity, gas, and water datasets into QuestDB for time-series analysis.

## Folder Structure

Kafka-QuestDB-Integration/
├── configs/             # Kafka Sink Connector JSON configs  
│   ├── electricity-sink.json  
│   ├── gas-sink.json  
│   └── water-sink.json  
├── data/                # Prepared 15-minute interval datasets  
│   ├── electricity.csv  
│   ├── gas.csv  
│   └── water.csv  
├── main.py              # Kafka Producer script (Python)  
├── pyproject.toml       # Environment setup (dependencies)  
├── uv.lock              # Package lockfile  

## Requirements

- Python 3.10+
- Kafka + Kafka Connect (running locally or via Docker)
- QuestDB (listening on port `9000`)

## Setup Instructions

1. Install dependencies:

   pip install -r requirements.txt  # or use poetry/uv with pyproject.toml

2. Start Kafka, Kafka Connect, and QuestDB (using Docker Compose or manually)

3. Configure QuestDB sink connectors using files in `configs/`
   - Register connectors with Kafka Connect REST API
   - Each config maps a Kafka topic to a QuestDB table:
     - `electricity_topic` → `electricity_consumption`
     - `gas_topic` → `gas_consumption`
     - `water_topic` → `water_consumption`

4. Run the Kafka producer to stream data:

   python main.py

## How It Works

- `main.py` uses Polars to read CSV files
- Streams each row to Kafka using `confluent_kafka.Producer`
- Each CSV file is streamed to its corresponding topic
- Kafka Connect sends messages to QuestDB using the sink connector

## Output Tables in QuestDB

- `electricity_consumption`
- `gas_consumption`
- `water_consumption`

## Authors

This module was developed as part of the campus digital twin project by the contributing team.
