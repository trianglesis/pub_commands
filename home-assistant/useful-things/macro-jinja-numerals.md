
# Macro


## Convert int to word

- https://github.com/jazzyisj/speech-helpers-jinja/blob/main/speech_helpers.jinja


```jinja
{%- macro number_words(number) -%}
{%- set number = number | replace(',', '') %}
{%- if number | contains('.') %}
  {%- set decimals = number.split('.')[1] | list %}
  {%- set number = (number | string).split('.')[0] | int(none) %}
{% else %}
  {%- set number = number | int(none) %}
{%- endif %}
{%- if is_number(number) %}
  {%- from 'speech_helpers.jinja' import number_words %}
  {%- set units = ["", "один", "два", "три", "чотири", "п'ять", "шість", "сім", "вісім", "дев'ять", "десять", "одинадцять ", "дванадцять", "тринадцять", "чорирнадцять", "п'ятнадцять", "шістнадцять", "сімнадцять", "вісімнадцять", "дев'ятнадцять"] %}
  {%- set tens = ["", "", "двадцять", "тридцять", "сорок", "п'ятдесят", "шістдесят", "сімдесят", "вісімдесят", "дев'яносто"] %}
  {%- set rem = number % 10 | int(-1) %}
  {%- if number == 0 -%} нуль
  {%- else %}
    {%- if number < 0 -%} мінус {% endif %}
    {%- if number < 20 -%} {%- if number < 0 -%}{{ units[int(number*-1)] }}{%- else %}{{ units[int(number)] }}{%- endif %}
    {%- elif number < 100 -%} {{ tens[number // 10] ~ iif(rem > 0, ' ' ~ units[rem], '') }}
    {%- elif number < 1000 -%} {{ number_words(number // 100) ~ ' сотня ' ~ iif(number % 100 > 0, number_words(number % 100), '') }}
    {%- elif number < 1000000 -%} {{ number_words(number // 1000) ~ ' тисяча ' ~ iif(number % 1000 > 0, number_words(number % 1000), '') }}
    {%- elif number < 1000000000 -%} {{ number_words(number // 1000000) ~ ' мільйон ' ~ iif(number % 1000000 > 0, number_words(number % 1000000), '') }}
    {%- elif number < 1000000000000 -%} {{ number_words(number // 1000000000) ~ ' мільярд ' ~ iif(number % 1000000000 > 0, number_words(number % 1000000000), '') }}
    {%- else -%} {{ number_words(number // 1000000000000) ~ ' трильйон ' ~ iif(number % 1000000000000 > 0, number_words(number % 1000000000000), '') }}
    {%- endif %}
    {%- if decimals is defined %}
    {{- ' крапка' -}}
      {%- for item in decimals %}
        {{- ' ' ~ number_words(decimals[loop.index0]) }}
      {%- endfor %}
    {%- endif %}
  {%- endif %}
{%- endif -%}
{%- endmacro -%}
```