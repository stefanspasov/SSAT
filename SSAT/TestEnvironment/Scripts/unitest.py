import socket
from sikuli import *
def assertExists(img):
  if(exists(img)):
    send("passed") 
  else:
    send("failed: " + img + " cannot be found on the screen.")

def send(MESSAGE):
  TCP_IP = '127.0.0.1'
  TCP_PORT = 8887
  BUFFER_SIZE = 65536
  s = socket.socket(socket.AF_INET, socket.SOCK_STREAM)
  s.connect((TCP_IP, TCP_PORT))
  s.send(MESSAGE)
  s.close()
  
def waitFor(img, secs):
  i = 0
  while (i<secs):
    if (exists(img)):
      send("passed")
      break
    i = i + 1
    sleep(1)
  if (i == secs):
    send("failed")