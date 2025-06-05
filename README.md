# 💻 KomputerowoE-commerce

Projekt systemu e-commerce z wykorzystaniem PostgreSQL jako bazy danych.

## 🧩 Struktura bazy danych

Poniżej znajduje się pełna definicja bazy danych, wraz z opisem tabel, sekwencji i relacji.

---

### 📌 Sekwencje 🔁

- `Order_id_seq` — sekwencja generująca unikalne ID dla tabeli `orders`.
- `product_id_seq` — sekwencja generująca unikalne ID dla tabeli `product`.

---

### 🗂️ Tabele

#### 1️⃣ orders 🛒

| Kolumna        | Typ                         | Opis                                                          |
|----------------|-----------------------------|---------------------------------------------------------------|
| `id`           | integer                     | Unikalny identyfikator (PK), generowany automatycznie przez `Order_id_seq`. |
| `orderdate`    | timestamp without time zone | Data i godzina złożenia zamówienia.                           |
| `customername` | character varying(100)      | Nazwa klienta, który złożył zamówienie.                       |

---

#### 2️⃣ product 📦

| Kolumna       | Typ                     | Opis                                                |
|---------------|-------------------------|-----------------------------------------------------|
| `id`          | integer                 | Unikalny identyfikator (PK), generowany automatycznie przez `product_id_seq`. |
| `name`        | character varying(100)  | Nazwa produktu.                                     |
| `price`       | numeric(10,2)           | Cena produktu.                                      |
| `type`        | character varying(50)   | Typ produktu (opcjonalne).                          |
| `description` | text                    | Opis produktu (opcjonalne).                         |

---

#### 3️⃣ orderproduct 🔗

| Kolumna    | Typ     | Opis                                              |
|------------|---------|---------------------------------------------------|
| `orderid`  | integer | Klucz obcy do `orders.id`.                         |
| `productid`| integer | Klucz obcy do `product.id`.                        |
| `quantity` | integer | Ilość zamówionych produktów.                       |

**Klucz główny:** złożony z `orderid` i `productid`.

---

## ⚙️ Instrukcja konfiguracji bazy danych 🛠️

1. **Zainstaluj PostgreSQL**  
   Pobierz i zainstaluj PostgreSQL ze strony: https://www.postgresql.org/download/

2. **Utwórz nową bazę danych**  
   Możesz to zrobić w konsoli `psql` lub za pomocą narzędzi graficznych typu pgAdmin.  
   Przykład w `psql`:

   ```bash
   createdb KomputerowoDB
   Uruchom skrypt tworzący strukturę bazy
3. ***W katalogu projektu powinien znajdować się plik schema.sql z definicjami tabel i sekwencji.***

Wykonaj polecenie:

  ```bash
  psql -U <twoj_uzytkownik> -d KomputerowoDB -f schema.sql
```

***Kod struktury tabel***
```sql
CREATE SEQUENCE public."Order_id_seq"
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;

CREATE SEQUENCE public.product_id_seq
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;

-- Tabela: orders
CREATE TABLE public.orders (
    id integer NOT NULL DEFAULT nextval('public."Order_id_seq"'::regclass),
    orderdate timestamp without time zone NOT NULL,
    customername character varying(100) NOT NULL,
    CONSTRAINT "Order_pkey" PRIMARY KEY (id)
);

-- Tabela: product
CREATE TABLE public.product (
    id integer NOT NULL DEFAULT nextval('public.product_id_seq'::regclass),
    name character varying(100) NOT NULL,
    price numeric(10,2) NOT NULL,
    type character varying(50),
    description text,
    CONSTRAINT product_pkey PRIMARY KEY (id)
);

-- Tabela: orderproduct
CREATE TABLE public.orderproduct (
    orderid integer NOT NULL,
    productid integer NOT NULL,
    quantity integer,
    CONSTRAINT orderproduct_pkey PRIMARY KEY (orderid, productid),
    CONSTRAINT orderproduct_orderid_fkey FOREIGN KEY (orderid)
        REFERENCES public.orders(id) ON DELETE CASCADE,
    CONSTRAINT orderproduct_productid_fkey FOREIGN KEY (productid)
        REFERENCES public.product(id) ON DELETE CASCADE
);
```
4. ***Warto zmienić***
   
   W pliku ([/appsettingsjson](https://github.com/Kufieta-Jakub/KomputerowoE-commerce/blob/master/KomputerowoE-commerce/appsettings.json))

   ```json
    "DefaultConnection": "Host=localhost;Port=5432;Database=KomputerowoDB;Username=<twój użytkownik>;Password=<twoje hasło>"

## 🔗 Jak się połączyć z wdrożoną aplikacją

Aplikacja została wdrożona na platformie **Azure App Service** i jest dostępna pod adresem:

```
https://komputerowo-bvbrhmccf6cxhddh.westeurope-01.azurewebsites.net
```

Link do swaggera:

```
https://komputerowo-bvbrhmccf6cxhddh.westeurope-01.azurewebsites.net/swagger
```

### 📡 Endpointy API

## METODY DLA PRODUKTU

- Lista produktów:  
  `GET /api/Product`
  
- Pobieranie produktów po id:  
`GET /api/Product/id/<id>`

- Lista produktów po name:  
  `GET /api/Product/name/<name>`

- Tworzenie produkty:  
  `POST /api/Product/createproduct`

- Edytowanie produkty:  
  `PATCH /api/Product/updateproduct/id/<id>`

- Usuwanie produktu:  
  `DELETE /api/Product/deleteproduct/id/<id>`

## METODY DLA ZAMÓWIENIA

- Lista zamówień:  
  `GET /api/Orders`

- Pobranie zamówienia po id:  
  `GET /api/Orders/{id}`
  
- Pobranie zamówienia po imieniu klienta:  
  `GET /api/Orders/customername/<customername>`
  
- Tworzenie zamówienia:  
  `POST /api/Orders/createorder`

- Edytowanie zamówienia
  `PATCH /api/Orders/updateorder/id/<id>`

- Usuwanie zamówienia:  
  `DELETE /api/Orders/deleteorder/id/<id>`

- Usuwanie produktu z zamówenia:  
  `/api/Orders/deleteproductorder/id/<id>/productid/<prductid>`

## ☁️ Wykorzystane usługi Azure

- **Azure App Service** – hostowanie backendu ASP.NET Core.
- **Azure Database for PostgreSQL Flexible Server** – baza danych aplikacji.
- **Azure Resource Group** – grupa do zarządzania zasobami.

---

## ⚙️ Konfiguracja

### appsettings.json

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Host=<HOSTNAME>;Port=5432;Database=<DATABASE_NAME>;Username=<USERNAME>;Password=<PASSWORD>;Ssl Mode=Require"
  },
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft.AspNetCore": "Warning"
    }
  },
  "AllowedHosts": "*"
}
```


```bash
# Przykład poprawnego JSON:
{
  "customername": "Jan Nowak",
  "orderproduct": [
    { "productid": 1, "quantity": 2 }
  ]
}
```

---
# Proces CI/CD
Polega on na automatycznym wdrożeniu i budowie ASP.NET core do Azure WEB APP za każdym razem gdy zrobi się push

##📁 Plik znajduje się w katalogu
```bash
.github/workflows/deploy.yml
```

**Proces jest wykonywany automatycznie po wypchnięciu push do gałęzi master**

** Build & Publish **
GitHub Actions wykonuje następujące kroki:

- Checkout kodu źródłowego

- Instalacja SDK .NET 8

- Przywrócenie zależności (dotnet restore)

- Kompilacja aplikacji (dotnet build)

- Publikacja aplikacji (dotnet publish) do katalogu ./publish


