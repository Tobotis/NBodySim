import numpy as np
import matplotlib.pyplot as plt
from matplotlib.animation import FuncAnimation
from matplotlib.colors import BASE_COLORS as colors
from mpl_toolkits.mplot3d import Axes3D
# Massen
masses = np.array([1, 1, 1])
# Positionen
positions = [[np.array([1, 1, 1])], [np.array([-1, 0, 0])], [
    np.array([1, 0, 0])]]
# Geschwindigkeiten
velocities = np.array([[0, 0, 0], [-1, 0, 0], [1, 0, 0]], dtype=np.float64)
# Weiteres
G = 1.0
dt = 0.01
T = 10

fig = plt.figure(figsize=(5, 5))
ax = fig.add_subplot(projection='3d')

iteration = 0
for time in np.arange(0, T, dt):
    for i in range(3):
        positions[i].append(
            positions[i][iteration]+velocities[i] * dt)
        sum = 0
        for j in range(3):
            if not j == i:
                sum += masses[j] * (positions[j][iteration] - positions[i][iteration]) / \
                    np.linalg.norm(
                        positions[j][iteration] - positions[i][iteration])**3
        sum *= G
        velocities[i] += sum * dt
    iteration += 1

bodies = [plt.plot([], [], [], color=list(colors)[i])[0]for i in range(3)]
lines = [plt.plot([], [], [], color=list(colors)[i], alpha=0.5)[0]
         for i in range(3)]


def animate(i):
    for j in range(3):
        lines[j].set_data([p[0] for p in positions[j][:i+1]], [p[1]
                          for p in positions[j][:i+1]], )
        lines[j].set_3d_properties([p[2] for p in positions[j][:i+1]])
        bodies[j].set_data(positions[j][i][0], positions[j][i][1])
        bodies[j].set_3d_properties(positions[j][i][2])
    ax.autoscale_view()
    return lines


anim = FuncAnimation(fig, animate,
                     frames=len(positions[0]), interval=5, blit=True)
plt.show()
