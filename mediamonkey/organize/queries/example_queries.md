https://www.mediamonkey.com/wiki/index.php/WebHelp:Configuring_Directory_and_File_Formats/5.0

## Auto org:

- `O:\sanek\Music\Foreign\`
- `O:\sanek\Music\Ukraine\`

### By Artist, Album [YEAR], Track number, Title:

- O:\sanek\Music\Foreign\<Artist>\<Album> [<Year>]\<Track #:2> <Title>
- O:\sanek\Music\Ukraine\<Artist>\<Album> [<Year>]\<Track #:2> <Title>

Difference in custom `Album types`

Album and Album type if exist
- .\<Artist>\<Album> [<Year>] $if(<Album type>,<Album type>)\<Track #:2> <Title>

Album type or jsut Album if exist
- .\<Artist>\$if(<Album type>,<Album type>, <Album>)[<Year>] \<Track #:2> <Title>

- O:\sanek\Music\Ukraine\<Artist>\<Album> [<Year>] $if(<Album type>,<Album type>)\<Track #:2> <Title>


### Use custom "Album Type" col

- `<Album type>`

- O:\sanek\Music\Foreign\$First(<Artist>)\<Album> [<Year>] <Album type>\<Track #:2> <Title>
- O:\sanek\Music\Foreign\<Artist>\<Album> [<Year>]\$Replace(<Folder:5>,<Album>,)\<Track #:2> <Title>


#### Alter

- O:\sanek\Music\Foreign\<Artist>\<Album> [<Year>] $if(<Album type>,<Album type>)\<Track #:2> <Title>
- O:\sanek\Music\Foreign\$First(<Artist>)\<Album> [<Year>] $if(<Album type>,<Album type>)\<Track #:2> <Title>

### Cut long string and spaces

\Music\<Artist>\$if(<Album>,<Album>,<Album Artist>)\<Track #:2> - $Replace($Trim($left(<Artist>, 35)), ,)

## From File path to tags:

### Create a Cutsom Album from Album sub directory

Use some dir from path as custom 'Albub type' name

- <Album type>\<Title>


- <Folder:5> - set a folder where Album name is indicate a type: `Live`, `compilation`, `EP`, `Country of origin`

- O:\sanek\Music\Foreign\<Artist>\<Album> [<Year>]\$Replace(<Folder:5>,<Artist> - <Year> - <Album>,)\<Track #:2> <Title>
- O:\sanek\Music\Foreign\<Artist>\<Album> [<Year>]\$Replace(<Folder:5>,<Artist> - <Year> - <Album>,)\<Track #:2> <Title>$Replace(<Folder:5>,<Artist> - <Year> - <Album>,)

# Workarounds:

Example path: `Artist\Album\Album Type\track.mp3`

1. Fill Custom `Album tag` from file path:

\<Album type>\<Title>
\<Album type>\<Ignore>

2. Now reorganize files:

Album and Album type if exist
.\<Artist>\<Album> [<Year>] $if(<Album type>,<Album type>)\<Track #:2> <Title>


# Hints:

```
$Replace(<Folder:5>,<Album>,) 
$if(string criteria,truevalue,falsevalue) 
```

# Etc

\Music\<Artist>\<Track #:2>

O:\sanek\Music\Foreign\<Artist>\<Album> [<Year>]\<Track #:2> <Title>
O:\sanek\Music\Foreign\$First(<Artist>)\<Album> [<Year>]\<Track #:2> <Title>
O:\sanek\Music\Foreign\<Artist>\<Album>\<Folder:5>\<Track #:2> <Title>
O:\sanek\Music\Foreign\$First(<Artist>)\<Album> [<Year>] $if(<Album type>,<Album type>)\<Track #:2> <Title>

\<Folder:1>\<Artist>\<Album> [<Year>]\<Track #:2> <Title>

O:\sanek\Music\Foreign\<Artist>\<Album> [<Year>]\$Replace(<Folder:5>,<Album>,)\<Track #:2> <Title>
O:\sanek\Music\Foreign\$First(<Artist>)\<Album> [<Year>] <Album type>\<Track #:2> <Title>

O:\sanek\Music\Foreign\<Artist>\<Album> [<Year>]\$Replace(<Folder:5>,<Artist> - <Year> - <Album>,)\<Track #:2> <Title>


O:\sanek\Music\Foreign\<Artist>\<Album> [<Year>]\$Replace(<Folder:5>,<Artist> - <Year> - <Album>,)\<Track #:2> <Title>$Replace(<Folder:5>,<Artist> - <Year> - <Album>,)
