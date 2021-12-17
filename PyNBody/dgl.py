import numpy as np
import matplotlib.pyplot as plt
from mpl_toolkits.mplot3d import Axes3D
from matplotlib import animation
np.random.seed(5)
# Gravitationskonstante
G = 1.0
COLORS = ["darkblue", "darkred", "darkgreen"]
# Zeitschritt
dT = 0.001
TIME = 200
# Ausgangsbedingungen für Körper 1
M1 = 1
X1 = np.array([0.0, 0.0, 0.0])
X1_D = np.array([0.0, 0.1, 0.0])
# Ausgangsbedingungen für Körper 2
M2 = 1
X2 = np.array([1, -1, 0.0])
X2_D = np.array([0.0, 0.0, -1])
# Ausgangsbedingungen für Körper 3
M3 = 1
X3 = np.array([-1, 1, 0.0])
X3_D = np.array([0.1, 0.0, 1])
print(X1, X2, X3)
# Differentialgleichungen


def de(x_1, x_2, x_3, body):
    if body == 0:
        return G * ((M2*(x_2-x_1))/(np.linalg.norm(x_2-x_1)**3) + (M3*(x_3-x_1))/(np.linalg.norm(x_3-x_1)**3))
    elif body == 1:
        return G * ((M3*(x_3-x_2))/(np.linalg.norm(x_3-x_2)**3) + (M1*(x_1-x_2))/(np.linalg.norm(x_1-x_2)**3))
    elif body == 2:
        return G * ((M1*(x_1-x_3))/(np.linalg.norm(x_1-x_3)**3) + (M2*(x_2-x_3))/(np.linalg.norm(x_2-x_3)**3))

# Lösung der Differential Gleichung


def x(t):
    vs = np.array([[[X1], X1_D], [
                  [X2], X2_D], [[X3], X3_D]], dtype=object)
    i = 0
    for time in np.arange(0, t, dT):
        for body in range(3):
            x_dd = de(vs[0][0][i], vs[1][0][i], vs[2][0][i], body)
            vs[body][0].append(vs[body][0][i] + vs[body][1] * dT)
            vs[body][1] = vs[body][1] + x_dd * dT
        i += 1
    return vs


# Plotting
if __name__ == "__main__":
    figure = plt.figure(figsize=(5, 5))
    # Solving numerically
    solution = x(TIME)
    ax = figure.add_subplot(111, projection="3d")

    for body in range(3):
        xs = [solution[body, 0][i][0] for i in range(len(solution[body, 0]))]
        ys = [solution[body, 0][i][1] for i in range(len(solution[body, 0]))]
        zs = [solution[body, 0][i][2] for i in range(len(solution[body, 0]))]
        ax.plot(xs, ys, zs, color=COLORS[body])
        ax.scatter(xs[-1], ys[-1], zs[-1], color=COLORS[body])
    ax.set_xlabel("x", fontsize=14)
    ax.set_ylabel("y", fontsize=14)
    ax.set_zlabel("z", fontsize=14)
    plt.show()
