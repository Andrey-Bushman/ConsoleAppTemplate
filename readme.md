# ConsoleAppTemplate

Пример консольного приложения, использующего `IHost` и CQRS.

Команды и запросы отделены от своих обработчиков за счёт вынесения их в
отдельные сборки. В конфигурационном файле нашего приложения указано, в каких
сборках медиатору следует искать команды и запросы, а в каких - их
обработчики (массив строк для каждой из  настроек).

В примере определено две команды и два их обработчика.