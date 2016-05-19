@echo off
SET KSPExe="D:\Program Files (x86)\Steam\steamapps\common\Kerbal Space Program\ksp.exe"
SET KSPDir="D:\Program Files (x86)\Steam\steamapps\common\Kerbal Space Program\GameData"
robocopy "GameData" %KSPDir% /E
start /min "" %KSPExe%
cls