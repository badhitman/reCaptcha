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

Внедрение:
Клиентскя/web часть интегрируется добавлением небольшим количеством скриптов. Можно внедрить в мастер страницу в самый конец _Layout.cshtml либо внедрив только на некоторые страницы
```html
<script src="https://www.google.com/recaptcha/api.js?render=reCaptchaV3PublicKey"></script>
<script>
	grecaptcha.ready(function()
	{
		var action = '@this.ViewContext.RouteData.Values["controller"]';
		grecaptcha.execute('reCaptchaV3PublicKey', {action: action}).then(function(token)
		{
			var data = new FormData();
			data.append("action", action);
			data.append("token", token);

			fetch('/reCaptcha3Verify', { method: 'POST', body: data }).then(function (response)
			{
				response.json().then(function (data)
				{
					// на клиенте можно как то отреагировать на оценку
                    if (!data || data.score < 0.9) {
                        //document.getElementById('re-captcha-form').style.display = 'block';
                    }
				});
			});
		});
	});
</script>
```
Разберём этот код:

- всё что внутри `grecaptcha.execute('reCaptchaV3PublicKey', {action: action}).then(function(token){...});` лучше разместить в отдельной функции в стороннем .js файле
- в этом коде `fetch('/reCaptcha3Verify', { method: 'POST', body: data })...` используется имя контроллера **reCaptcha3Verify** для проверки reCaptcha v3. Если имя данного контроллера в вашем случае другое, то отразите нужное имя.
- v3 подразумевает, что при каждом запросе оценки пользователя нужно ещё сообщить какая область сайта проверяется. reCaptcha настоятельно рекомендует сверять этот параметр в ответах сервера. Я использую универсальный подход `var action = '@this.ViewContext.RouteData.Values["controller"]';`
- `reCaptchaV3PublicKey` это ваш публичный ключ reCaptcha. В вашем проекте вместо этой строки должен быть реальный ключ

## Серверная часть немного больше.
 
Для удобства работы с сессиями потребуется расширение его функционала:
```c#
public static class SessionExtensions
{
	public static void Set<T>(this ISession session, string key, T value)
	{
		session.SetString(key, JsonConvert.SerializeObject(value));
	}

	public static T Get<T>(this ISession session, string key)
	{
		var value = session.GetString(key);

		return value == null ? default(T) : JsonConvert.DeserializeObject<T>(value);
	}
}
```

Ещё потребуется унаследовать и реализовать абстрактный класс `reCaptcha3VerifyController`. Вам потребуется как-то обеспечить назначение приватного ключа.
Пример простейшей реализации:
```c#
public class ReCaptcha3VerifyController : reCaptcha.reCaptcha3VerifyController
{
	public override string reCaptchaV3PrivatKey { get; }
	blic ReCaptcha3VerifyController(IOptions<AppConfig> _options)
	{
		reCaptchaV3PrivatKey = _options.Value.reCaptchaV3PrivatKey;
	}
}
```

В общем и целом серверную часть можно описать так:

- Контроллер получает и обработывает токены от клиента
- Вместе с токеном в запросе от web клиента мы получаем в том числе имя действия
- В случае удачной проверки токена во временном кеше сохраняется результат этой проверки. По умолчанию результат сохраняется на 2 минуты в MemoryCache. Срок хранения можно изменить через `reCaptcha3VerifyController.CacheSuccessVerifyResultLifetimeMinutes`
- В любом методе контроллера можно попытаться получить значение проверки токена.

Пример использования:
```c#
public ActionResult Login()
{
	string client_id = HttpContext.Session.Get<string>("ClientId");
	if (!string.IsNullOrWhiteSpace(client_id))
	{
		byte[] reCaptchaBody;
		if (cache.TryGetValue(client_id, out reCaptchaBody))
		{
			if (reCaptchaBody is null || reCaptchaBody.Length == 0)
			{
				// можно десереализовать и ознакомиться с оценкой текущего пользователя
			}
			// исключаем повторное использование одноразового/временного токена
            cache.Remove(client_id);
		}
	}
	return View();
}
```


В реализации используются MemoryCache
```c#
services.AddDistributedMemoryCache();
services.AddMemoryCache();
```

В реализации используются сессии
```c#
services.AddSession(options =>
{
	options.Cookie.Name = ".MyApp.Session";
	options.IdleTimeout = TimeSpan.FromMinutes(60);
});
```

Кроме того тут же можно ознакомится с примером реализации IActionFilter reCaptcha3StateFilter. Применение этого этого фильтра сам ведёт контроль жизненного цикла токенов.
Доступ к объекту оценки токена упрощён до `reCaptcha3ResponseModel reCaptcha3Status = HttpContext.Session.Get<reCaptcha3ResponseModel>(typeof(reCaptcha3StateFilter).Name);`
```c#
[ServiceFilter(typeof(reCaptcha3StateFilter))]
public IActionResult Register()
{
	reCaptcha3ResponseModel reCaptcha3Status = HttpContext.Session.Get<reCaptcha3ResponseModel>(typeof(reCaptcha3StateFilter).Name);
	return View();
}
```
Объект оценки токена будет автоматически удалён из кеша после отработки метода.

## v2
Общие условия. В для удачной привязки токена к модели - потребуется свойство:

```c#
public string g_recaptcha_response { get; set; }
```
Наш привязчик свяжет поле web формы g-recaptcha-response с нашей моделью.

Этот токен можно проверить так:
```C#
reCaptcha2ResponseModel my_verifier = reCaptcha.stat.reCaptchaVerifier.reCaptcha2SiteVerify("ваш_приватный_ключ", g_recaptcha_response, "user_ip")
```


## v2 Checkbox widget
Эта версия предполагает, что вы явно вставите тег **DIV** внутри формы в нужном месте. Благодаря такой вставки к нашей форме при отправке будет добавлено дополнительное поле с именем g-recaptcha-response.
Тут важно обратить внимание что имя поля имеет символы дефиса. Просто так привязать такое поле к модели не получится из за того что именам полей самой модели нельзя иметь дефисы.
Для того что бы поле g-recaptcha-response автоматически переименовывались в g_recaptcha_response на этапе привязки модели нужно добавить в ваш проект соответсвующий провайдер привязчика
```C#
services.AddMvc(opts =>
{
	opts.ModelBinderProviders.Insert(0, new CustomReCaptcha2ResponseBinderProvider());
});
```
Соответсвующий провайдер и привязчик к нему есть в проекте. Достаточно его только подключить в вашем проекте.
После подключения этого провайдера поля форм с именем g-recaptcha-response будут переименованы в g_recaptcha_response.
Другими словами: в web формах допускается иметь тег input с именем "g-recaptcha-response". Такое поле автоматически будет привязано к свойству модели с именем g_recaptcha_response.

## v2 Invisible
Этот вид капчи самостоятельно решает: запрашивать от пользователя прохождение тестов или нет.
Если пользователь вызывает доверие у reCaptcha, то в отправляемой форме вы получите для проверки валидный токен как будто пользователя проверили, но реально этой формы пользователь не увидит.
Если пользователь не вызывает доверия у reCaptcha, то при отправке формы в центре окна выйдет окошко для прохождения проверки, а на сервер будет отправлен токен этой проверки

###### v2 Android
Я не имел практики использования. Реализации этой версии в данном решении нет
