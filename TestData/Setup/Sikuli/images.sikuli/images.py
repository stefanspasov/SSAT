from sikuli import *
menu = Region(5,1,1909,45)

leftArrow = menu.inside().find("1424090179539.png")
leftArrowSlow = "1424090179539.png" 
sectionsWindow = Pattern("1424097341567.png").similar(0.51)