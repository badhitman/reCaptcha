# reCaptcha
reCaptcha asp.net core

Решение для внедрения на свой сайт reCaptcha

Версий reCaptcha на текущий момент две (**v2** и **v3**), но **v2** имеет две подверсии.
- v3 - эта версия не подразумевает ни какой капчи. Эта версия только оценивает пользователя по шкале от 0 до 1, где оценкой 0 обозначаются например боты или спамеры, а 1 оценка для реального человека. Оценку производит сам гугл
- v2 - классические задачки с выбором картинок и так далее
    - "стандартный" в явном виде обычного чекбокса
	- "скрытый", когда reCaptcha сама решает показывать чекбокс или нет. Тоесть если решит что капчу лучше не показывать, то вы получите для проверки валидный токен какбудто человек проходил проверку и прошёл ее удачно