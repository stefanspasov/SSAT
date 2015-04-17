import socket
from sikuli import *
def assertExists(img, step, port):
  if(exists(img)):
    send("STEP:" + step + ":passed") 
  else:
    send("STEP:" + step + ":failed")
    
def send(MESSAGE):
    TCP_IP = '127.0.0.1'
    TCP_PORT = 8887
    BUFFER_SIZE = 65536
    s = socket.socket(socket.AF_INET, socket.SOCK_STREAM)
    s.connect((TCP_IP, TCP_PORT))
    s.send(MESSAGE)
    s.close()  
  
  