import numpy as np
import matplotlib.pyplot as plt
import matplotlib.animation as animation
from matplotlib.colors import BASE_COLORS as colors
from mpl_toolkits.mplot3d import Axes3D as p3

ANIMATE = False
# Massen
masses = np.array([1, 1, 1])
# Positionen
positions = [[np.array([10, 1, 1])], [np.array(
    [-1, 10, 0])], [np.array([1, 0, 0])]]
# Geschwindigkeiten
velocities = np.array([[0, 1, 0], [-5, 0, 0], [1, 0, -2]], dtype=np.float64)
# Weiteres
G = 10.0
dt = 0.01
T = 200
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


fig = plt.figure(figsize=(5, 5))
ax = fig.add_subplot(111, projection='3d',)

xs = []
ys = []
zs = []

for body in range(3):
    xs.append([positions[body][i][0] for i in range(iteration)])
    ys.append([positions[body][i][1] for i in range(iteration)])
    zs.append([positions[body][i][2] for i in range(iteration)])
ax.set_xlabel("x", fontsize=14)
ax.set_ylabel("y", fontsize=14)
ax.set_zlabel("z", fontsize=14)

if not ANIMATE:
    # Plotting
    for body in range(3):
        ax.plot(xs[body], ys[body], zs[body], color=list(colors)[body])
        ax.scatter(xs[body][-1], ys[body][-1], zs[body]
                   [-1], color=list(colors)[body])

    plt.show()
else:
    dots = [ax.scatter(xs[i][0], ys[i][0], zs[i][0],
                       color=list(colors)[i]) for i in range(3)]

    def animate(i, xs, ys, zs, dots):
        for body in range(3):
            dots[body].remove()
            dots[body] = ax.scatter(
                xs[body][i], ys[body][i], zs[body][i], color=list(colors)[body])
            ax.plot(xs[body][:i+1], ys[body][:i+1],
                    zs[body][:i+1], color=list(colors)[body])

    anim = animation.FuncAnimation(
        fig, animate, frames=iteration, fargs=(xs, ys, zs, dots), blit=False, interval=1)
    writervideo = animation.FFMpegWriter(fps=30)
    anim.save("test.mp4", writer=writervideo)
