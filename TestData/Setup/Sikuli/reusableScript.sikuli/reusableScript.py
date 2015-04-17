import unitest
from unitest import *
from sikuli import *
import images
from images import *
reload(images)
def clickArrow():
  click(leftArrow)
  assertExists(sectionsWindow,"1","p")