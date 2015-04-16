### What is this?

This is a lightweight tool (mainly for livestreamers) to easily get what's being played in osu! (http://osu.ppy.sh/) into a file. This is aimed to help the viewers that want to know the song name of the map that's being played without having to ask in chat. However, all this does is read the title of the osu! window and gets basic song information from that. Nothing fancy.

### How do I use it?

Simply download a compiled binary from the downloads list below. Double-clicking it will have it run with the normal settings. However, supplying the following format: "KEY=VALUE" (without quotations, and don't use spaces) as part of the arguments when running the executable allows you to change two variables. The "delay" variable (default: 250) changes how often the program refreshes (measured in milliseconds). So something like "OsuSongName.exe delay=1000" runs the program the delay being 1000 milliseconds, or 1 second. Another variable that can be changed is the "file" (default: "C:/Users/{USER_DIR}/Desktop/osu_song_name.txt" ({USER_DIR} denotes the current user)). So something like "OsuSongName.exe file=c:/users/Me/songName.txt" will successfully change the file path. Of course, you can modify both variables at the same time.

### Downloads

* LATEST (15 Apr. 2015): http://www.mediafire.com/download/qh6d8wbwy2158xw/OsuSongName.exe)