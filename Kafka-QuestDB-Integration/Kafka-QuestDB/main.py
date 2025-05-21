import json
import polars as pl

from time import sleep
from confluent_kafka import Producer
from concurrent.futures import ThreadPoolExecutor

csv_topic_map = {
    'water.csv': 'water_topic',
    'electricity.csv': 'electricity_topic',
    'gas.csv': 'gas_topic'
}

conf = {'bootstrap.servers': 'localhost:9092'}
producer = Producer(conf)


def push_csv_to_kafka(file_path, topic):
    print(f"Sending {file_path} → {topic}")

    df = pl.read_csv(f'data/{file_path}')
    df = df.fill_null(0).sort(pl.col('timestamp'))

    for row in df.iter_rows(named=True):
        message = json.dumps(row)
        producer.produce(topic, value=message.encode('utf-8'), callback=delivery_report)
        producer.poll(0)
       # sleep(1)

    producer.flush()

    print(f"Finished: {file_path} → {topic}")


def delivery_report(err, msg):
    """ Called once for each message produced to indicate delivery result.
        Triggered by poll() or flush(). """
    if err is not None:
        print('Message delivery failed: {}'.format(err))
    else:
        print('Message delivered to {} [{}]'.format(msg.topic(), msg.partition()))


def main():
    with ThreadPoolExecutor(max_workers=3) as executor:
        futures = [
            executor.submit(push_csv_to_kafka, file_path, topic)
            for file_path, topic in csv_topic_map.items()
        ]
        for future in futures:
            future.result()


if __name__ == '__main__':
    main()
