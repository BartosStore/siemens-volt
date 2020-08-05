Příprava prostředí

Pro jednoduché otestování doporučuji použít lokální IIS a SQL server Express. Během konfigurace IIS vytvořit nový application pool, novou webovou aplikaci s vlastním adresářem pro deploy z Visual Studia. V SQL serveru je potřeba vytvořit novou DB a aplikovat přiložený init_db.sql skript.

Connection string je definovaný v appsettings.json. Proveďte prosím jeho úpravu podle Vašeho SQL serveru a DB:
"SqlDbConnection": "Server=<název_server>;Database=<název_db>;Trusted_Connection=True"

Nasazení aplikace

Ve Visual Studiu zvolit Publish do adresáře. Target location je místo na disku, kde IIS webová aplikace očekává zkompilovaný kód. Configuration Release. Provést Publish. V tuto chvíli se zkompilovaná aplikace nachází na tzv. Physical Path; typicky: C:\inetpub\wwwroot\<název_web_aplikace>.

Při nastavení Pool Identity na ApplicationPoolIdentity bude do databáze přistupovat vlastník procesu. Přístup do DB tedy musí mít uživatel 'IIS APPPOOL\<název_web_aplikace>'. Aplikace je ošetřena proti podobným chybám. Pro případné ladění povolte ExceptionPage a ErrorPage v Startup.cs - metodě Configure().

Testování

Aplikace naběhne na veřejném View. Přejděte prosím k registraci, loginu a poté na stránky Audit a Odběr. Audit zobrazuje zalogovaná data z DB s možností detailu, filtrace, sortování a stránkování. Odběr nabízí generování hodnot pomocí JS skriptu.