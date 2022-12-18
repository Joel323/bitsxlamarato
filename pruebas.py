import cv2
import matplotlib.pyplot as plt
import numpy as np

vidcap = cv2.VideoCapture('circDolent.avi')
i=0
while True:
    ret, frame = vidcap.read()
    if ret:
        cv2.imwrite("2aneurisme%d.jpg" %i, frame)
    else:
        break
    i+=1
cv2.waitKey(0)
vidcap.release()
cv2.destroyAllWindows()