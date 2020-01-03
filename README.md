<h3 align="center">
  LogicalDocWrapper
</h3>

[![License: GPL v2](https://img.shields.io/badge/License-GPL%20v2-blue.svg)](https://www.gnu.org/licenses/old-licenses/gpl-2.0.en.html)

LogicalDocWrapper is an open source, lightweight document management system wrapper for LogicalDoc community edition.

<hr />
<h2 align="center">
  ✨ <a href="https://github.com/users/swissbyte/sponsorship">Sponsor this project if you use and appreciate it!</a> ✨
</h2>
<hr />


Features
--------

- Starts and stops tomcat server
- Starts and stops MariaDB / Mysql Server


#### Requirements
- Java 11 (JRE55)
- JAVA_HOME environment variable set properly
- .NET Framework installed (for LocicalDocWrapper)

Installation
-------------------

First of all download your copy of LogicalDoc community edition from here: 
https://sourceforge.net/projects/logicaldoc/files/distribution/LogicalDOC%20CE%208.3/logicaldoc-8.3.4-tomcat-bundle.zip/download
Of course, use the latest available one. 
Extract the content of this zip to your prefered directory. For example: C:\temp\ld

Next download mariadb:
https://downloads.mariadb.org/
Also extract this to your prefered folder. For example: C:\temp\ld\mariadb

Now start LocicalDocWraper.exe and go to settings. 
Choose the two directories and go to home and select Start LogicalDoc

Now the server should start and you should be able to connect to http://localhost:8080

If you need to change the port, go into the LocalDoc folder under config/server.xml. 
There you can change the port. 

If you were able to connect (this takes up to 2 minutes) you should go on with the setup (link at the bottom of the page)
Now you should also create a new database within your mariadb instance. For this you could use a MYSQL Client like MYSQL Workbench. 

After that you should be ready to use your instance. 

Look at this guide for more informations: https://wiki.logicaldoc.com/wiki/Quick_Install

Future features
-------------------

Planned features: 

- Backup
- Automatic upgrade


Contributing
------------

All contributions are more than welcomed. Contributions may close an issue, fix a bug (reported or not reported), improve the existing code, add new feature, and so on.

The `master` branch is the default and base branch for the project. It is used for development and all Pull Requests should go there.

License
-------

Teedy is released under the terms of the GPL license. See `COPYING` for more
information or see <http://opensource.org/licenses/GPL-2.0>.
