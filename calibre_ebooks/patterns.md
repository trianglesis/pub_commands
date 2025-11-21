# Format file system pattern for sync

Doc:
- https://manual.calibre-ebook.com/template_lang.html

```python
calibre/{author_sort:shorten(25,,0)}/{series:||/}{series:|-|}{series_index:|-|}/{title}
calibre/{author_sort:shorten(25,,0)}/{series:|-|}{series_index:|-|}/{title}
```

## aa

Series by subdir:
```python
# aa - subdir when series exists for one serie
{series}/{series_index} - {title}

# bb - subdir per single title
{title}/{title}

calibre/{author_sort:shorten(25,,0)}/{series:lookup(.,#aa,#bb)}
{series:lookup(.,#aa,#bb)}
```


