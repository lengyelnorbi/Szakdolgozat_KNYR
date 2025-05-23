Program használata/tesztelése

A futáshoz szükséges adatbázist lokálisan létre kell hozni - a nyilvantarto_rendszer.sql fájl importálásával.
Az adatbázis elérését beállítani a RepositoryBase.cs _connectionString értékével lehet beállítani.

Az adatbázis importálásával a felhasználók is létrejönnek, amikhez a felhasználónév és jelszó elérhető a usernamePwd_test.txt fájlban.
A lennor felhasználónév az egyetlen ADMIN szerepű felhasználó.