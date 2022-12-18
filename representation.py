import numpy as np
from imageio import imread
from matplotlib import pyplot as plt
import morphsnakes as ms

def representation(background, result, count):
    plt.close()
    fig = plt.figure()
    fig.clf()
    ax1 = fig.add_subplot(1, 2, 1)
    ax1.imshow(background, cmap=plt.cm.gray)

    ax2 = fig.add_subplot(1, 2, 2)
    ax_u = ax2.imshow(np.zeros_like(background), vmin=0, vmax=1)
    plt.pause(0.001)
    if ax1.collections:
        del ax1.collections[0]
    ax1.contour(result, [0.5],colors='r')
    ax_u.set_data(result)
    fig.canvas.draw()
    plt.savefig("result%d.jpg" %count)
    plt.pause(0.001)