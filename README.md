# Siemens Volt

- aplikace generuje data o spotřebě elektrické energie pomocí JS skriptu
- data jsou ukládána v SQL server databázi
- obsluhující stránky jsou dostupné pouze přihlášeným uživatelům
- logovány jsou akce generování, přihlášení, login

## Audit

- výpis dat z DB
- umožňuje sortování, filtrování a stránkování na straně BE

![audit](assets/audit.PNG "Audit")

## Odběr

- spouští/zastavuje generování hodnot z JS skriptu
- data jsou odeslána do kontrolleru k uložení do DB
- k dispozici je status a aktuální hodnota

![odber](assets/odber.PNG "Odber")

## Registrace

- registrace vynucuje pomocí validací korektní email, shodná hesla a minimální/maximální délku
- úspěšná registrace je zalogována

![registrace](assets/registrace.PNG "Registrace")

## Přihlášení

- vstup na akční obsah webu je dostupný pouze přihlášeným uživatelům
- úspěšné přihlášení je zalogováno

![prihlaseni](assets/prihlaseni.PNG "Prihlaseni")