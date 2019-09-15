# reCaptcha
reCaptcha // c# asp.net core 2.2

Решение для внедрения на свой сайт reCaptcha

Версий reCaptcha на текущий момент две (**v2** и **v3**), но **v2** имеет три под-версии.
- [v3](https://developers.google.com/recaptcha/docs/v3) - даёт оценку для каждого посещения пользователем страниц вашего сайта без какого либо взаимодействия с самим пользователем
- v2 - задачки с выбором картинок и прочие проверочные "тесты зрения и нервов пользователя"
    - ["стандартный"](https://developers.google.com/recaptcha/docs/display) для расположение в явном виде привычного "чекбокса" в вашей форме. Например, для "дополнительной защиты" форм авторизации или регистрации
	- ["скрытый"](https://developers.google.com/recaptcha/docs/invisible), когда reCaptcha сама решает запрашивать/показывать задачки или нет
	- [android](https://developer.android.com/training/safetynet/recaptcha.html), который вы можете использовать для защиты своего приложения от вредоносного трафика.
	
	
## v3
Оценка основана на взаимодействиях пользователя с вашим сайтом и позволяет вам предпринять соответствующие решения.
Таким образом можно вводить или пропускать те или иные уровни защиты или дополнительные проверки на вашем сайте.
Эта версия не подразумевает ни какой капчи в принципе. Собственно, называть эту версией капчей не уместно.
Версия **v3** только оценивает пользователя по шкале от 0 до 1, где:
- 0 обозначаются например боты или спамеры
- 1 оценка для "хорошего" пользователя

Оценка между 0 и 1 означает, что reCaptcha не уверен с кем имеет дело и судя по значению можно понять к чему reCaptcha больше склоняется.

## v2 Checkbox widget


## v2 Invisible
То есть если решит что капчу лучше не показывать, то вы получите для проверки валидный токен как будто человек проходил проверку и прошёл ее удачно

###### v2 Android
Я не имел практики использования. Реализации этой версии в данном решении нет
