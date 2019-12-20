# HomeFileRenamer

This is a home project to rename (video) files along with the following rules.

## The Problem
I have a lots of home pictures and videos. I backup them separately in two destination(in fact to three, into two identical HDD and online). Usually i sort the picture first into separate directories. I generate the directory name in the following way:

**[YYYY.MM.DD] - [Description]**

ex.:

**2019.02.03 - Gödöllői állatkert és Gyöngyösoroszi kőzet műzeum**

At the end I have a nice directory structure with the pictures in it, without the video files. The video files are usually in a separate directory with names like this:
* 2019-01-17 11.27.44.mp4
* 2019-01-17 11.28.49.mp4

## The goal
I have one directory full of "unnamed" videos and another with the nice directory names. I wanted to 
1. match the video filename with a directory (ex.: 2019-01-18 12.46.04.mp4 => 2019.01.18 - Eplényi síelés)
2. add the description to the video file from the directory (ex.: 2019-01-18 12.46.04.mp4 => 2019-01-18 12.46.04 - Eplényi síelés.mp4)
