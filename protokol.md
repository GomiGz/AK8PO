
## Scénář
Přišel za námi zákazník a chtěl by vytvořit aplikaci, která mu umožní pořádat turnaje v bojovém umění.

## Požadavky
- Evidence závodníků - databáze obsahující jejich jméno, příjmení, e-mail, věk a technický stupeň (10kyu ----- 1 kyu - 1 dan ------ 10 dan)
- Při organizaci závodu je omezení na věk a technický stupeň - omezení z hora i z dola - tito se pak účastní turnaje
- Při spuštění dojde k rozdělění do 3 člených poolů
  - Vítěz prvního poolu se utká s druhým z posledního poolu
  - Vítěz posledního poou se utká s druhým z prvního poolu
  - Závodníci poté postupují v single eliminination bracket dokud nezbyde jeden vítěz
- Aplikace musí umožnňovat vyplňovat výsledky v poolech a pavoukovi
#### Utkání
- Zápasí se na 2 vítězné body nebo do vypršení 3 minut
  - M, D, K, T
- H - napomenutí
    - pokud dostane hráč 2 napomenutí, změní se v 1 vítězný bod
- X - remíza
- E - prodloužení
- oo - odstoupení     

## Technologie
- Windows forms (uživatelské prostředí)
- SQLite databáze (Relační databáze)
- Dapper (Připojení aplikace k DB)
- Nlog (Logování 

## Časový plán
| Aktivita | Předpokládaná<br>časová náročnost<br>(hod) | Konečná<br>časová náročnost<br>(hod) |
|----------|--------------------------------------------|--------------------------------------|
| Vyslechnutí požadavků a rozsahu prací | 1 | 1 |
| Sepsání protokolu | 4 | 3 |
| Design architektury | 3 | 2 |
| Návrh datového modelu | 2 | 2|
| Příprava prostředí | 1 | 1 |
| Zprovoznění a vytvoření databáze | 2 | 15 |
| Vytvoření správy závodníků | 5 | 5 |
| Vytvoření stránky pro spuštění turnaje | 2 | 2|
| Generování turnajového rozlosování | 3 |2 |
| Vytvoření grafického zobrazení pavouka | 10 | |
| Vyplňování výsledků utkání | 4 | |
| Rozšíření výsledků utkání o konkrétní bodová hodnocení | 3 | |
| **Celkem** | **40**  |  |

## Otázky
##### Co když není počet závodníků dělitelný třemi ?
Pokud by v jednom poolu zůstali pouze dva závodníci, automaticky postupují, je pouze třeba rozhodnout o jejich pořadí. 
V případě, že by zůstal pouze jeden závodník, dojde namísto toho k rozdělení do 2 poolu po 2.
##### Co když vyjde lichý počet poolů ?
Jeden ze závodníků přeskakuje jednu úroveň pavouka.
##### Jak se losuje ?
Losuje se úplně náhodně.
##### Co když mají v poolu stejný počet výher ?
Rozhoduje počet získaných vítězných bodů.
