from flask import Flask, jsonify
import threading, random, time

app = Flask(__name__)

room_data = [
    {"roomId": "Room_A101", "temperature": 24.0, "electricity": "ON"},
    {"roomId": "Room_B202", "temperature": 29.0, "electricity": "OFF"}
]

def simulate_data():
    while True:
        for room in room_data:
            room["temperature"] = round(random.uniform(20.0, 32.0), 1)
            room["electricity"] = random.choice(["ON", "OFF"])
        time.sleep(10)

# Run simulation in background
threading.Thread(target=simulate_data, daemon=True).start()

@app.route('/api/rooms')
def get_rooms():
    return jsonify(room_data)

if __name__ == '__main__':
    app.run(host='0.0.0.0', port=5000)
