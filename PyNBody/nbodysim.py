import numpy as np
import matplotlib.pyplot as plt
from matplotlib.animation import FuncAnimation
from matplotlib.colors import BASE_COLORS as colors
# Massen
masses = np.array([1, 1, 1])
# Positionen
positions = np.array([[[0, 0, 0]], [[0, 0, 0]], [[0, 0, 0]]])
# Geschwindigkeiten
velocities = np.array([[0, 0, 0], [0, 0, 0], [0, 0, 0]])
# Weiteres
G = 1.0
dt = 0.01
T = 200

fig = plt.figure(figsize=(5, 5))
ax = fig.add_subplot(projection = '3d')

iteration = 0
for time in range(0,T,dt):
    for i in range(3):
        positions[i][iteration] = velocities[i] * dt
        sum = 0
        for j in range(3):
            if not j == i:
                sum += masses[j] * (positions[j] - positions[i]) / np.linalg.norm(positions[j] - positions[i])**3
        sum *= G
        velocities[i] += sum *dt 
    iteration += 1

bodies = np.array([plt.plot([],[], [], color=list(colors)[i])[0] for i in range(3)])
lines = np.array([plt.plot([],[], [], color=list(colors)[i],alpha=0.5)[0] for i in range(3)])



