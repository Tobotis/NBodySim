import numpy as np
a = np.array([[[1, 2, 3], [4, 5, 6]], [[1, 2, 3], [3, 4, 5]]],
             dtype=np.float64)
b = np.array([1, 2, 3], dtype=np.float64)
a[0] = np.concatenate((a[0], b))
print(a, a.shape)
