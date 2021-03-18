import os
import platform
import sys

project_dir = sys.argv[1]
target_dir = sys.argv[2]

if(platform.system() == "Darwin"):
  os.system("xcopy /e /i /y {project_dir}fonts {target_dir}fonts&#xD;&#xA;xcopy /e /i /y {project_dir}assets {target_dir}assets".format(target_dir = target_dir, project_dir = project_dir))
else:
  os.system("cp -R {project_dir}fonts {target_dir}; cp -R {project_dir}assets {target_dir}".format(target_dir = target_dir, project_dir = project_dir))
