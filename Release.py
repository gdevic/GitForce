#!/usr/bin/env python3
# Release script for MS Windows
#
# * Runs MSBuild to compile GitForce release version
# * Increments the version number and sets the build date in AssemblyInfo.cs file
# * Gets the log of changes since last release and formats changes.txt to be used as a release commit text
#
import sys, time, datetime, os, subprocess, re, shutil

def IncrementVersion(file, change):
    # Open the changelist file to write
    fch = open(change, "wt")
    # Save the original version file since it will be modified
    file_backup = file+"~"
    if(os.path.exists(file_backup)):
        os.remove(file_backup)
    os.rename(file, file_backup)
    fin = open(file_backup)
    fout = open(file, "wt")
    # Find version and build date and update them
    for line in fin:

        if line.find("AssemblyFileVersion")>0:
            p = re.compile(r'\W+')
            ver = p.split(line)
            print('Major = ', ver[3])
            print('Minor = ', ver[4])
            print('Build = ', ver[5], ' -> ', int(ver[5])+1)
            release = ver[3]+'.'+ver[4]+'.'+str(int(ver[5])+1)
            line = line.replace(ver[3]+'.'+ver[4]+'.'+ver[5], release)

        if line.find("AssemblyProduct")>0:
            now = datetime.datetime.now()
            format = "%Y/%m/%d"
            built = now.strftime(format)
            print('Setting the Build date = ', built)
            line = "[assembly: AssemblyProduct(\"GitForce built on " + built + "\")]\n"

        fout.write(line)
    fin.close()
    fout.close()
    fch.write("### Release " + release + " (" + built + ")\n")
    fch.close();
    return release

def IncrementVersionXml(file, version):
    # Save the original file before modifying it
    file_backup = file+"~"
    if(os.path.exists(file_backup)):
        os.remove(file_backup)
    os.rename(file, file_backup)
    fin = open(file_backup)
    fout = open(file, "wt")
    # Find version and build date and update them
    for line in fin:
        pos = line.find("<version>")
        if pos > 0:
            line = " " * pos
            line = line + "<version>" + version + "</version>\n"
        fout.write(line)
    fin.close()
    fout.close()

version_file = "Properties/AssemblyInfo.cs"
changelist_file = "change.txt"

version = IncrementVersion(version_file, changelist_file)
print('Building:')
subprocess.Popen(["MSBuild.exe", "/t:Rebuild", "/p:Configuration=Release"], shell=True).wait()

# Copy released executable
shutil.copy2("obj/x86/Release/GitForce.exe", "./") # Here, to the root of the project

# Get the summary of changes and append to the subject
proc = subprocess.Popen(["git", "log", "--format=%B"], stdout=subprocess.PIPE)
stdout = proc.communicate()[0].decode('windows-1252')
stdout = stdout[0:stdout.find("###")]

h = open(changelist_file, "a")
h.write("-------------------------------------------\n")
h.write("SUMMARY OF CHANGES:\n\n")
h.write(stdout)
h.close()

print('-------------------------------------------')
print('Submit ' + version_file)
print('using the description in ' + changelist_file)
