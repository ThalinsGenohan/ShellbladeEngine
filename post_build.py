import os
import platform
import sys

project_dir = sys.argv[1]
target_dir = sys.argv[2]

if(platform.system() == "Windows"):
  os.system("rmdir {target_dir}assets /s /q & xcopy {project_dir}assets {target_dir}assets /i /s /e /y".format(target_dir = target_dir, project_dir = project_dir))
else:
  os.system("rm -rf {target_dir}assets;cp -R {project_dir}assets {target_dir}".format(target_dir = target_dir, project_dir = project_dir))
