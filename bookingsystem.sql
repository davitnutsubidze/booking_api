--
-- PostgreSQL database dump
--

\restrict NBINFb3xNj7UfU3p4odrVD6aaOzijfBOjtf9zwAlzzqhB8GssfkpYMVaggWe1EC

-- Dumped from database version 18.1
-- Dumped by pg_dump version 18.1

SET statement_timeout = 0;
SET lock_timeout = 0;
SET idle_in_transaction_session_timeout = 0;
SET transaction_timeout = 0;
SET client_encoding = 'UTF8';
SET standard_conforming_strings = on;
SELECT pg_catalog.set_config('search_path', '', false);
SET check_function_bodies = false;
SET xmloption = content;
SET client_min_messages = warning;
SET row_security = off;

SET default_tablespace = '';

SET default_table_access_method = heap;

--
-- Name: Appointments; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public."Appointments" (
    "Id" uuid NOT NULL,
    "TenantId" uuid CONSTRAINT "Appointments_BusinessId_not_null" NOT NULL,
    "ServiceId" uuid NOT NULL,
    "StaffId" uuid NOT NULL,
    "CustomerId" uuid NOT NULL,
    "StartDateTime" timestamp with time zone NOT NULL,
    "EndDateTime" timestamp with time zone NOT NULL,
    "Status" integer NOT NULL,
    "Notes" text,
    "CreatedAt" timestamp with time zone NOT NULL,
    "UpdatedAt" timestamp with time zone
);


ALTER TABLE public."Appointments" OWNER TO postgres;

--
-- Name: BlockedTimes; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public."BlockedTimes" (
    "Id" uuid CONSTRAINT "BlockedTime_Id_not_null" NOT NULL,
    "TenantId" uuid CONSTRAINT "BlockedTime_BusinessId_not_null" NOT NULL,
    "StaffId" uuid,
    "StartDateTimeUtc" timestamp with time zone CONSTRAINT "BlockedTime_StartDateTime_not_null" NOT NULL,
    "EndDateTimeUtc" timestamp with time zone CONSTRAINT "BlockedTime_EndDateTime_not_null" NOT NULL,
    "Reason" text,
    "CreatedAt" timestamp with time zone CONSTRAINT "BlockedTime_CreatedAt_not_null" NOT NULL,
    "UpdatedAt" timestamp with time zone
);


ALTER TABLE public."BlockedTimes" OWNER TO postgres;

--
-- Name: Customers; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public."Customers" (
    "Id" uuid CONSTRAINT "Customer_Id_not_null" NOT NULL,
    "TenantId" uuid CONSTRAINT "Customer_BusinessId_not_null" NOT NULL,
    "FirstName" text CONSTRAINT "Customer_FirstName_not_null" NOT NULL,
    "LastName" text CONSTRAINT "Customer_LastName_not_null" NOT NULL,
    "Phone" text CONSTRAINT "Customer_Phone_not_null" NOT NULL,
    "Email" text,
    "Notes" text,
    "CreatedAt" timestamp with time zone CONSTRAINT "Customer_CreatedAt_not_null" NOT NULL,
    "UpdatedAt" timestamp with time zone
);


ALTER TABLE public."Customers" OWNER TO postgres;

--
-- Name: Payment; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public."Payment" (
    "Id" uuid NOT NULL,
    "AppointmentId" uuid NOT NULL,
    "Amount" numeric NOT NULL,
    "Currency" text NOT NULL,
    "Status" integer NOT NULL,
    "Provider" text NOT NULL,
    "TransactionId" text,
    "CreatedAt" timestamp with time zone NOT NULL,
    "UpdatedAt" timestamp with time zone
);


ALTER TABLE public."Payment" OWNER TO postgres;

--
-- Name: Services; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public."Services" (
    "Id" uuid CONSTRAINT "Service_Id_not_null" NOT NULL,
    "TenantId" uuid CONSTRAINT "Service_BusinessId_not_null" NOT NULL,
    "Name" text CONSTRAINT "Service_Name_not_null" NOT NULL,
    "Description" text,
    "DurationMinutes" integer CONSTRAINT "Service_DurationMinutes_not_null" NOT NULL,
    "Price" numeric,
    "Currency" text,
    "IsActive" boolean CONSTRAINT "Service_IsActive_not_null" NOT NULL,
    "CreatedAt" timestamp with time zone CONSTRAINT "Service_CreatedAt_not_null" NOT NULL,
    "UpdatedAt" timestamp with time zone
);


ALTER TABLE public."Services" OWNER TO postgres;

--
-- Name: Staff; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public."Staff" (
    "Id" uuid NOT NULL,
    "TenantId" uuid CONSTRAINT "Staff_BusinessId_not_null" NOT NULL,
    "UserId" uuid,
    "FirstName" text NOT NULL,
    "LastName" text NOT NULL,
    "Phone" text,
    "Bio" text,
    "IsActive" boolean NOT NULL,
    "CreatedAt" timestamp with time zone NOT NULL,
    "UpdatedAt" timestamp with time zone
);


ALTER TABLE public."Staff" OWNER TO postgres;

--
-- Name: StaffLunchBreaks; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public."StaffLunchBreaks" (
    "Id" uuid NOT NULL,
    "TenantId" uuid NOT NULL,
    "StaffId" uuid NOT NULL,
    "DayOfWeek" integer NOT NULL,
    "StartTime" time without time zone NOT NULL,
    "EndTime" time without time zone NOT NULL,
    "IsEnabled" boolean NOT NULL
);


ALTER TABLE public."StaffLunchBreaks" OWNER TO postgres;

--
-- Name: StaffServices; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public."StaffServices" (
    "StaffId" uuid NOT NULL,
    "ServiceId" uuid NOT NULL
);


ALTER TABLE public."StaffServices" OWNER TO postgres;

--
-- Name: StaffWorkingHours; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public."StaffWorkingHours" (
    "Id" uuid NOT NULL,
    "StaffId" uuid NOT NULL,
    "DayOfWeek" integer NOT NULL,
    "StartTime" time without time zone NOT NULL,
    "EndTime" time without time zone NOT NULL,
    "IsDayOff" boolean NOT NULL
);


ALTER TABLE public."StaffWorkingHours" OWNER TO postgres;

--
-- Name: Tenant; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public."Tenant" (
    "Id" uuid NOT NULL,
    "Name" character varying(200) NOT NULL,
    "Slug" character varying(80) NOT NULL,
    "Description" text,
    "Phone" text NOT NULL,
    "Email" text NOT NULL,
    "Address" text NOT NULL,
    "TimeZone" text NOT NULL,
    "IsActive" boolean NOT NULL,
    "CreatedAt" timestamp with time zone NOT NULL,
    "UpdatedAt" timestamp with time zone
);


ALTER TABLE public."Tenant" OWNER TO postgres;

--
-- Name: TenantWorkingHours; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public."TenantWorkingHours" (
    "Id" uuid NOT NULL,
    "TenantId" uuid CONSTRAINT "TenantWorkingHours_BusinessId_not_null" NOT NULL,
    "DayOfWeek" integer NOT NULL,
    "StartTime" time without time zone NOT NULL,
    "EndTime" time without time zone NOT NULL,
    "IsClosed" boolean NOT NULL
);


ALTER TABLE public."TenantWorkingHours" OWNER TO postgres;

--
-- Name: User; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public."User" (
    "Id" uuid NOT NULL,
    "TenantId" uuid,
    "FirstName" text NOT NULL,
    "LastName" text NOT NULL,
    "Email" text NOT NULL,
    "PasswordHash" text NOT NULL,
    "Role" integer NOT NULL,
    "IsActive" boolean NOT NULL,
    "CreatedAt" timestamp with time zone NOT NULL,
    "UpdatedAt" timestamp with time zone
);


ALTER TABLE public."User" OWNER TO postgres;

--
-- Name: __EFMigrationsHistory; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public."__EFMigrationsHistory" (
    "MigrationId" character varying(150) NOT NULL,
    "ProductVersion" character varying(32) NOT NULL
);


ALTER TABLE public."__EFMigrationsHistory" OWNER TO postgres;

--
-- Data for Name: Appointments; Type: TABLE DATA; Schema: public; Owner: postgres
--

COPY public."Appointments" ("Id", "TenantId", "ServiceId", "StaffId", "CustomerId", "StartDateTime", "EndDateTime", "Status", "Notes", "CreatedAt", "UpdatedAt") FROM stdin;
c3423821-62a2-42e1-9bb6-77f6f27c4c57	11111111-1111-1111-1111-111111111111	ffa90e3a-ef53-48fc-9586-2db50ef1324d	f16b15e8-da67-4b58-8c0f-bafe32b82f16	cbc101de-195a-4a54-9fb5-d6208b808a3a	2026-03-11 09:00:00+04	2026-03-11 10:00:00+04	0	\N	2026-03-09 21:47:51.429186+04	\N
a4529ef9-356e-466f-a6d9-b4254e845391	11111111-1111-1111-1111-111111111111	33022636-ec36-4681-a774-b666d6fd4ce5	fc5483cc-c44b-4486-a7e7-a6f6b5c07bcb	cbc101de-195a-4a54-9fb5-d6208b808a3a	2026-03-11 09:00:00+04	2026-03-11 10:30:00+04	0	\N	2026-03-09 21:49:06.530739+04	\N
2c120d68-5de6-430b-ad02-fdbf201462d3	11111111-1111-1111-1111-111111111111	ffa90e3a-ef53-48fc-9586-2db50ef1324d	f16b15e8-da67-4b58-8c0f-bafe32b82f16	cbc101de-195a-4a54-9fb5-d6208b808a3a	2026-03-10 15:00:00+04	2026-03-10 16:00:00+04	0	test notes	2026-03-09 21:57:34.907395+04	\N
36d55a4a-97dc-4879-af07-58957b1b2145	11111111-1111-1111-1111-111111111111	ffa90e3a-ef53-48fc-9586-2db50ef1324d	f16b15e8-da67-4b58-8c0f-bafe32b82f16	cbc101de-195a-4a54-9fb5-d6208b808a3a	2026-03-10 16:00:00+04	2026-03-10 17:00:00+04	0	test notes	2026-03-09 21:59:27.19362+04	\N
7b924c4f-8050-42cd-a45a-471e38c587e0	11111111-1111-1111-1111-111111111111	b5c169d5-952b-4831-bf3c-4acde507e3bb	6d8e1d19-353e-4404-b966-1ed025c2f834	cbc101de-195a-4a54-9fb5-d6208b808a3a	2026-03-12 13:00:00+04	2026-03-12 15:00:00+04	0	test notes	2026-03-09 22:04:04.463111+04	\N
946bce28-e638-4155-9c60-1ffd011f23de	11111111-1111-1111-1111-111111111111	ffa90e3a-ef53-48fc-9586-2db50ef1324d	b34c3375-e393-4b1b-ad5c-41c2c16e6794	8b27a47a-f943-470c-bf40-b2e235d89863	2026-03-16 10:00:00+04	2026-03-16 11:00:00+04	0	\N	2026-03-09 22:12:16.784815+04	\N
ee00b94f-feb6-47ec-874e-a162fa009fda	11111111-1111-1111-1111-111111111111	33022636-ec36-4681-a774-b666d6fd4ce5	b34c3375-e393-4b1b-ad5c-41c2c16e6794	8b27a47a-f943-470c-bf40-b2e235d89863	2026-03-16 15:00:00+04	2026-03-16 16:30:00+04	0	\N	2026-03-09 22:12:55.643269+04	\N
\.


--
-- Data for Name: BlockedTimes; Type: TABLE DATA; Schema: public; Owner: postgres
--

COPY public."BlockedTimes" ("Id", "TenantId", "StaffId", "StartDateTimeUtc", "EndDateTimeUtc", "Reason", "CreatedAt", "UpdatedAt") FROM stdin;
\.


--
-- Data for Name: Customers; Type: TABLE DATA; Schema: public; Owner: postgres
--

COPY public."Customers" ("Id", "TenantId", "FirstName", "LastName", "Phone", "Email", "Notes", "CreatedAt", "UpdatedAt") FROM stdin;
01e5a5e5-ac1f-4bf7-8ccd-0677b87458cf	11111111-1111-1111-1111-111111111111	Dato	Test	+995555111222	dato@test.ge	\N	2026-02-18 18:56:53.216956+04	\N
0924057d-d171-4872-9fd0-470274bd6764	11111111-1111-1111-1111-111111111111	test	test	598506636	test@gmail.com	\N	2026-02-25 18:54:56.844541+04	\N
28318626-8219-4c24-a681-eafa8dc97f26	11111111-1111-1111-1111-111111111111	DAVIT	NUTSUBIDZE	+995598506636	davit.nutsubidz@Gmail.com	\N	2026-03-09 14:42:17.706055+04	\N
cbc101de-195a-4a54-9fb5-d6208b808a3a	11111111-1111-1111-1111-111111111111	Tata	11-09:00	595027575	tamar.mukhadgverdeli@gmail.com	\N	2026-03-09 21:47:51.34868+04	\N
8b27a47a-f943-470c-bf40-b2e235d89863	11111111-1111-1111-1111-111111111111	test	test	7678687688	tamar.mukhadgverdeli@gmail.com	\N	2026-03-09 22:12:16.78138+04	\N
\.


--
-- Data for Name: Payment; Type: TABLE DATA; Schema: public; Owner: postgres
--

COPY public."Payment" ("Id", "AppointmentId", "Amount", "Currency", "Status", "Provider", "TransactionId", "CreatedAt", "UpdatedAt") FROM stdin;
\.


--
-- Data for Name: Services; Type: TABLE DATA; Schema: public; Owner: postgres
--

COPY public."Services" ("Id", "TenantId", "Name", "Description", "DurationMinutes", "Price", "Currency", "IsActive", "CreatedAt", "UpdatedAt") FROM stdin;
db000530-622b-4435-a30d-2f28ad3eab7e	11111111-1111-1111-1111-111111111111	მანიკური - შილაკი	შილაკი	90	50	GEL	t	2026-03-04 16:00:33.263181+04	\N
33022636-ec36-4681-a774-b666d6fd4ce5	11111111-1111-1111-1111-111111111111	პედიკური -  კლასიკური	კლასიკური	90	50	GEL	t	2026-03-04 15:59:53.530012+04	\N
fd7a7ff1-6fd0-4d1b-9d5e-674c5f7bdc89	11111111-1111-1111-1111-111111111111	პედიკური - შილაკი	შილაკი	100	80	GEL	t	2026-03-04 16:00:57.404904+04	\N
b5c169d5-952b-4831-bf3c-4acde507e3bb	11111111-1111-1111-1111-111111111111	შეღებვა - ერთ ფერში		120	100	GEL	t	2026-03-04 16:03:22.025483+04	\N
ffa90e3a-ef53-48fc-9586-2db50ef1324d	11111111-1111-1111-1111-111111111111	მანიკური - კლასიკური	კლასიკური	60	30	GEL	t	2026-03-04 15:29:45.69761+04	\N
\.


--
-- Data for Name: Staff; Type: TABLE DATA; Schema: public; Owner: postgres
--

COPY public."Staff" ("Id", "TenantId", "UserId", "FirstName", "LastName", "Phone", "Bio", "IsActive", "CreatedAt", "UpdatedAt") FROM stdin;
f16b15e8-da67-4b58-8c0f-bafe32b82f16	11111111-1111-1111-1111-111111111111	\N	Tata	Test			t	2026-03-04 15:28:52.789734+04	\N
6d8e1d19-353e-4404-b966-1ed025c2f834	11111111-1111-1111-1111-111111111111	\N	Tata2	Test			t	2026-03-04 15:52:03.227922+04	\N
5ab0410d-ad7b-4df3-ae16-fe3f66fbbba9	11111111-1111-1111-1111-111111111111	\N	Tata3	Test			t	2026-03-04 16:04:17.00522+04	\N
fc5483cc-c44b-4486-a7e7-a6f6b5c07bcb	11111111-1111-1111-1111-111111111111	\N	Tata4	Test			t	2026-03-04 16:04:45.072426+04	\N
b3e339fe-f072-425d-92c7-8e2f9b4da522	11111111-1111-1111-1111-111111111111	\N	Tata5	Test			t	2026-03-04 16:05:05.779127+04	\N
ca0f9545-7af8-4745-9fa1-240d95776dcb	11111111-1111-1111-1111-111111111111	\N	dat	123123			t	2026-03-05 14:00:58.966538+04	\N
b34c3375-e393-4b1b-ad5c-41c2c16e6794	11111111-1111-1111-1111-111111111111	\N	ყველაფერშიკი	ტესტ	087487280707		t	2026-03-09 22:11:22.875978+04	\N
\.


--
-- Data for Name: StaffLunchBreaks; Type: TABLE DATA; Schema: public; Owner: postgres
--

COPY public."StaffLunchBreaks" ("Id", "TenantId", "StaffId", "DayOfWeek", "StartTime", "EndTime", "IsEnabled") FROM stdin;
18897058-7ffe-4b70-b4b9-5e836a4f2b82	11111111-1111-1111-1111-111111111111	f16b15e8-da67-4b58-8c0f-bafe32b82f16	4	13:00:00	14:00:00	t
1ed8a3bb-2865-41e9-a7e7-1cd5db4bd6c4	11111111-1111-1111-1111-111111111111	f16b15e8-da67-4b58-8c0f-bafe32b82f16	0	13:00:00	14:00:00	t
1fb5bf3e-2bba-4558-8c2b-ddff98bf36a0	11111111-1111-1111-1111-111111111111	f16b15e8-da67-4b58-8c0f-bafe32b82f16	5	13:00:00	14:00:00	t
76b0c157-cdc9-45f8-b14b-43edf4c71dd6	11111111-1111-1111-1111-111111111111	f16b15e8-da67-4b58-8c0f-bafe32b82f16	1	13:00:00	14:00:00	t
a12d8b23-5ba9-4907-bcd8-be857001afa4	11111111-1111-1111-1111-111111111111	f16b15e8-da67-4b58-8c0f-bafe32b82f16	6	13:00:00	14:00:00	t
a95de5b8-2c5d-4fb7-b8ae-9be7d35598a1	11111111-1111-1111-1111-111111111111	f16b15e8-da67-4b58-8c0f-bafe32b82f16	3	13:00:00	14:00:00	t
feb94f43-fe88-43a2-bb77-7694f2b1c20b	11111111-1111-1111-1111-111111111111	f16b15e8-da67-4b58-8c0f-bafe32b82f16	2	13:00:00	14:00:00	t
2e477ee3-bd39-4a41-8906-ac19d0772904	11111111-1111-1111-1111-111111111111	5ab0410d-ad7b-4df3-ae16-fe3f66fbbba9	0	13:00:00	14:00:00	t
5668671c-9b7e-44da-a000-1108cb56c037	11111111-1111-1111-1111-111111111111	5ab0410d-ad7b-4df3-ae16-fe3f66fbbba9	6	13:00:00	14:00:00	t
b9da3634-c7d0-4cd2-86f7-6f357169953c	11111111-1111-1111-1111-111111111111	5ab0410d-ad7b-4df3-ae16-fe3f66fbbba9	4	13:00:00	14:00:00	t
c082f225-f543-4c5a-9b1f-209e1f38ee84	11111111-1111-1111-1111-111111111111	5ab0410d-ad7b-4df3-ae16-fe3f66fbbba9	5	13:00:00	14:00:00	t
c2f81cd6-58cb-4f6b-ad26-bc2632a0001b	11111111-1111-1111-1111-111111111111	5ab0410d-ad7b-4df3-ae16-fe3f66fbbba9	2	13:00:00	14:00:00	t
f2d3b0a2-a4d1-40a2-8013-3d577e001c2c	11111111-1111-1111-1111-111111111111	5ab0410d-ad7b-4df3-ae16-fe3f66fbbba9	1	13:00:00	14:00:00	t
f585d5f0-cbe1-4899-b13c-bc918dfea987	11111111-1111-1111-1111-111111111111	5ab0410d-ad7b-4df3-ae16-fe3f66fbbba9	3	13:00:00	14:00:00	t
0a452f7d-9c3e-4a07-954c-e1221c7e0965	11111111-1111-1111-1111-111111111111	b3e339fe-f072-425d-92c7-8e2f9b4da522	4	13:00:00	14:00:00	t
19eec015-67ff-4092-b8d7-d7c84d18155c	11111111-1111-1111-1111-111111111111	b3e339fe-f072-425d-92c7-8e2f9b4da522	0	13:00:00	14:00:00	t
3d2f22b8-9079-4fa7-9d2f-a264e80d02b7	11111111-1111-1111-1111-111111111111	b3e339fe-f072-425d-92c7-8e2f9b4da522	1	13:00:00	14:00:00	t
51a4566b-09ca-46ae-8958-24c1b805de74	11111111-1111-1111-1111-111111111111	b3e339fe-f072-425d-92c7-8e2f9b4da522	5	13:00:00	14:00:00	t
66457cc3-fb51-4a55-a828-f070dc78eb65	11111111-1111-1111-1111-111111111111	b3e339fe-f072-425d-92c7-8e2f9b4da522	2	13:00:00	14:00:00	t
d841b7db-ae3a-4106-97c2-530cffc63c8a	11111111-1111-1111-1111-111111111111	b3e339fe-f072-425d-92c7-8e2f9b4da522	3	13:00:00	14:00:00	t
ebdafc74-5a7c-4a6b-9a39-db20e51de694	11111111-1111-1111-1111-111111111111	b3e339fe-f072-425d-92c7-8e2f9b4da522	6	13:00:00	14:00:00	t
a22ddd1d-92f7-4ff0-a167-d9b546db3c51	11111111-1111-1111-1111-111111111111	fc5483cc-c44b-4486-a7e7-a6f6b5c07bcb	5	14:00:00	15:00:00	t
a7a22873-cd9a-44d6-a954-53ffa4b808a2	11111111-1111-1111-1111-111111111111	6d8e1d19-353e-4404-b966-1ed025c2f834	0	12:00:00	13:00:00	t
55815c7d-29be-4d59-86e2-3750890878ca	11111111-1111-1111-1111-111111111111	6d8e1d19-353e-4404-b966-1ed025c2f834	1	12:00:00	13:00:00	t
ce8a0780-569b-4237-b87e-12afb0713164	11111111-1111-1111-1111-111111111111	fc5483cc-c44b-4486-a7e7-a6f6b5c07bcb	6	14:00:00	15:00:00	t
4aff592a-f018-4fa0-8c7b-af8339613a9a	11111111-1111-1111-1111-111111111111	6d8e1d19-353e-4404-b966-1ed025c2f834	2	12:00:00	13:00:00	t
0fa954d2-a566-41d0-ae12-b3eb80e677e3	11111111-1111-1111-1111-111111111111	ca0f9545-7af8-4745-9fa1-240d95776dcb	0	13:00:00	14:00:00	t
97bf6100-d256-4fc9-9d3b-49887ffdaa48	11111111-1111-1111-1111-111111111111	6d8e1d19-353e-4404-b966-1ed025c2f834	3	12:00:00	13:00:00	t
3ad0ae54-5691-4494-9d1e-62d4fce29875	11111111-1111-1111-1111-111111111111	ca0f9545-7af8-4745-9fa1-240d95776dcb	5	13:00:00	14:00:00	t
463bc1a2-1297-47f2-bc63-8ec9dc697ab1	11111111-1111-1111-1111-111111111111	6d8e1d19-353e-4404-b966-1ed025c2f834	4	12:00:00	13:00:00	t
45302bfc-0b3c-4d58-863c-ee38ed0257f4	11111111-1111-1111-1111-111111111111	ca0f9545-7af8-4745-9fa1-240d95776dcb	6	13:00:00	14:00:00	t
f46ab0ed-be64-41f0-a547-77785845f477	11111111-1111-1111-1111-111111111111	6d8e1d19-353e-4404-b966-1ed025c2f834	5	12:00:00	13:00:00	t
47e89850-d6e5-41e5-91a5-8cc67c89a111	11111111-1111-1111-1111-111111111111	ca0f9545-7af8-4745-9fa1-240d95776dcb	4	13:00:00	14:00:00	t
c821effe-9636-486b-bb7f-fd53da08f6c9	11111111-1111-1111-1111-111111111111	6d8e1d19-353e-4404-b966-1ed025c2f834	6	12:00:00	13:00:00	t
846380c0-c05c-46b3-b68b-eab96ad52275	11111111-1111-1111-1111-111111111111	ca0f9545-7af8-4745-9fa1-240d95776dcb	3	13:00:00	14:00:00	t
a3ed303c-c13b-4ac0-a258-f84bf9eb8c86	11111111-1111-1111-1111-111111111111	ca0f9545-7af8-4745-9fa1-240d95776dcb	2	13:00:00	14:00:00	t
ca81bcb5-5df3-4ffc-8e4e-cbb2699a8c4d	11111111-1111-1111-1111-111111111111	fc5483cc-c44b-4486-a7e7-a6f6b5c07bcb	0	14:00:00	15:00:00	t
f165a7ac-fa2e-4c50-abbe-2fd197056680	11111111-1111-1111-1111-111111111111	ca0f9545-7af8-4745-9fa1-240d95776dcb	1	13:00:00	14:00:00	t
f403fa57-00a4-42c4-a84c-0cf34c1a0ab3	11111111-1111-1111-1111-111111111111	fc5483cc-c44b-4486-a7e7-a6f6b5c07bcb	1	14:00:00	15:00:00	t
c0241f1b-cc62-4a20-9c1d-2c160f01c69f	11111111-1111-1111-1111-111111111111	fc5483cc-c44b-4486-a7e7-a6f6b5c07bcb	2	14:00:00	15:00:00	t
b165e553-acd6-4f8d-b4d4-78b4e4a1bafe	11111111-1111-1111-1111-111111111111	fc5483cc-c44b-4486-a7e7-a6f6b5c07bcb	3	14:00:00	15:00:00	t
54b0c0fe-72ed-4c4c-b2ef-ba359ecb8e3c	11111111-1111-1111-1111-111111111111	fc5483cc-c44b-4486-a7e7-a6f6b5c07bcb	4	14:00:00	15:00:00	t
12b18cf1-40b1-403a-a62a-5f5c88f381fd	11111111-1111-1111-1111-111111111111	b34c3375-e393-4b1b-ad5c-41c2c16e6794	2	13:00:00	14:00:00	t
34024061-0ebc-47db-a547-1ffaebb17397	11111111-1111-1111-1111-111111111111	b34c3375-e393-4b1b-ad5c-41c2c16e6794	4	13:00:00	14:00:00	t
6ad4f16a-f1c0-46ce-b037-4312b2f70619	11111111-1111-1111-1111-111111111111	b34c3375-e393-4b1b-ad5c-41c2c16e6794	3	13:00:00	14:00:00	t
6ba68659-193b-413b-b27c-3ee4106d9bc0	11111111-1111-1111-1111-111111111111	b34c3375-e393-4b1b-ad5c-41c2c16e6794	1	13:00:00	14:00:00	t
8502c975-21c0-4fe6-83a9-525accec9516	11111111-1111-1111-1111-111111111111	b34c3375-e393-4b1b-ad5c-41c2c16e6794	6	13:00:00	14:00:00	t
8ee6e088-3c23-4008-8e8f-adca5004b2a8	11111111-1111-1111-1111-111111111111	b34c3375-e393-4b1b-ad5c-41c2c16e6794	0	13:00:00	14:00:00	t
fc7d29ec-8be6-49f2-b145-6072775c3fbe	11111111-1111-1111-1111-111111111111	b34c3375-e393-4b1b-ad5c-41c2c16e6794	5	13:00:00	14:00:00	t
\.


--
-- Data for Name: StaffServices; Type: TABLE DATA; Schema: public; Owner: postgres
--

COPY public."StaffServices" ("StaffId", "ServiceId") FROM stdin;
f16b15e8-da67-4b58-8c0f-bafe32b82f16	ffa90e3a-ef53-48fc-9586-2db50ef1324d
f16b15e8-da67-4b58-8c0f-bafe32b82f16	33022636-ec36-4681-a774-b666d6fd4ce5
6d8e1d19-353e-4404-b966-1ed025c2f834	b5c169d5-952b-4831-bf3c-4acde507e3bb
5ab0410d-ad7b-4df3-ae16-fe3f66fbbba9	ffa90e3a-ef53-48fc-9586-2db50ef1324d
5ab0410d-ad7b-4df3-ae16-fe3f66fbbba9	db000530-622b-4435-a30d-2f28ad3eab7e
fc5483cc-c44b-4486-a7e7-a6f6b5c07bcb	ffa90e3a-ef53-48fc-9586-2db50ef1324d
fc5483cc-c44b-4486-a7e7-a6f6b5c07bcb	db000530-622b-4435-a30d-2f28ad3eab7e
fc5483cc-c44b-4486-a7e7-a6f6b5c07bcb	33022636-ec36-4681-a774-b666d6fd4ce5
fc5483cc-c44b-4486-a7e7-a6f6b5c07bcb	fd7a7ff1-6fd0-4d1b-9d5e-674c5f7bdc89
b3e339fe-f072-425d-92c7-8e2f9b4da522	b5c169d5-952b-4831-bf3c-4acde507e3bb
b34c3375-e393-4b1b-ad5c-41c2c16e6794	ffa90e3a-ef53-48fc-9586-2db50ef1324d
b34c3375-e393-4b1b-ad5c-41c2c16e6794	db000530-622b-4435-a30d-2f28ad3eab7e
b34c3375-e393-4b1b-ad5c-41c2c16e6794	33022636-ec36-4681-a774-b666d6fd4ce5
b34c3375-e393-4b1b-ad5c-41c2c16e6794	fd7a7ff1-6fd0-4d1b-9d5e-674c5f7bdc89
b34c3375-e393-4b1b-ad5c-41c2c16e6794	b5c169d5-952b-4831-bf3c-4acde507e3bb
\.


--
-- Data for Name: StaffWorkingHours; Type: TABLE DATA; Schema: public; Owner: postgres
--

COPY public."StaffWorkingHours" ("Id", "StaffId", "DayOfWeek", "StartTime", "EndTime", "IsDayOff") FROM stdin;
e1621fa8-dda9-4391-b04e-7a7ad7210c45	2a64c28c-f7ab-4ece-9bd5-08c6e8157b9d	1	10:00:00	16:00:00	f
c9da1d43-11f5-43b6-aea3-ea092c6c5b9f	2a64c28c-f7ab-4ece-9bd5-08c6e8157b9d	3	10:00:00	13:00:00	t
58838d2e-c683-468a-91c6-1769c8932d16	8a267aaf-1b5d-439d-bc29-ecf99bdd682f	1	10:50:00	15:50:00	f
099df410-e857-45d0-a3e1-169dedc36a6f	f16b15e8-da67-4b58-8c0f-bafe32b82f16	1	10:00:00	17:00:00	f
cf0c365f-9893-47b5-8df4-d589a2744f4b	f16b15e8-da67-4b58-8c0f-bafe32b82f16	2	15:00:00	20:00:00	f
5772c6b5-eb2c-4137-a357-4b114f9b26db	6d8e1d19-353e-4404-b966-1ed025c2f834	3	16:43:00	21:43:00	t
\.


--
-- Data for Name: Tenant; Type: TABLE DATA; Schema: public; Owner: postgres
--

COPY public."Tenant" ("Id", "Name", "Slug", "Description", "Phone", "Email", "Address", "TimeZone", "IsActive", "CreatedAt", "UpdatedAt") FROM stdin;
11111111-1111-1111-1111-111111111111	Men's Barber	ink-zone	Tattoo studio	+995500000000	hello@inkzone.ge	Tbilisi	Asia/Tbilisi	t	2026-02-18 18:26:41.0957+04	\N
\.


--
-- Data for Name: TenantWorkingHours; Type: TABLE DATA; Schema: public; Owner: postgres
--

COPY public."TenantWorkingHours" ("Id", "TenantId", "DayOfWeek", "StartTime", "EndTime", "IsClosed") FROM stdin;
acfe525f-50a3-4207-9d1c-ed3212ddadd9	11111111-1111-1111-1111-111111111111	5	09:00:00	18:00:00	f
2359731c-c095-4291-a215-d3fb98d983fa	33333333-3333-3333-3333-333333333333	4	10:00:00	16:00:00	f
2ccfefcf-0d33-4851-95aa-a29efcb4c945	33333333-3333-3333-3333-333333333333	0	10:00:00	16:00:00	f
846a2ce7-3e85-442a-9e0b-4b23f584be6f	33333333-3333-3333-3333-333333333333	5	10:00:00	16:00:00	f
8e7bcb39-a386-4fff-a134-1c28c8fbdf21	33333333-3333-3333-3333-333333333333	1	10:00:00	16:00:00	f
b2405ea0-40f8-4500-9f4d-be49b9005058	33333333-3333-3333-3333-333333333333	6	10:00:00	16:00:00	f
d1b1bd54-5ef9-4edf-98bf-4d74bff13880	33333333-3333-3333-3333-333333333333	2	10:00:00	16:00:00	f
e78f935b-b222-43a4-9332-3e2f2e8bbbeb	33333333-3333-3333-3333-333333333333	3	10:00:00	16:00:00	f
dc49b084-ed75-4a94-aca8-e8d635eb1607	11111111-1111-1111-1111-111111111111	6	09:00:00	18:00:00	t
e8672b62-0068-4dd5-a615-ac936ff27ac6	11111111-1111-1111-1111-111111111111	3	09:00:00	18:00:00	f
7d63615d-c701-43e9-ad0c-afa14364b03b	11111111-1111-1111-1111-111111111111	4	09:00:00	18:00:00	f
17de9fa9-dfd9-4506-af28-ed089d05545e	11111111-1111-1111-1111-111111111111	2	09:00:00	18:00:00	f
022a4ebd-2dea-4845-a9ac-091c3604ffcf	11111111-1111-1111-1111-111111111111	0	12:00:00	16:00:00	t
34a50fad-3135-49c7-8a3f-544d70471ed4	11111111-1111-1111-1111-111111111111	1	09:00:00	18:00:00	f
\.


--
-- Data for Name: User; Type: TABLE DATA; Schema: public; Owner: postgres
--

COPY public."User" ("Id", "TenantId", "FirstName", "LastName", "Email", "PasswordHash", "Role", "IsActive", "CreatedAt", "UpdatedAt") FROM stdin;
\.


--
-- Data for Name: __EFMigrationsHistory; Type: TABLE DATA; Schema: public; Owner: postgres
--

COPY public."__EFMigrationsHistory" ("MigrationId", "ProductVersion") FROM stdin;
20260218124305_InitialCreate	10.0.3
20260218145609_CreateServicesTable	10.0.3
20260218162118_AddWorkingHoursAndBlockedTime	10.0.3
20260218163817_RenameBlockedTimeUtcFields	10.0.3
20260218173945_changeBusinesstoTenant	10.0.3
20260218181134_replaceAllBusinessToTenant	10.0.3
20260218181737_replaceAllBusinessToTenantName	10.0.3
20260220152537_Unique_StaffWorkingHours_StaffId_DayOfWeek	10.0.3
20260223103836_AddStaffLunchBreaks	10.0.3
\.


--
-- Name: Appointments PK_Appointments; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public."Appointments"
    ADD CONSTRAINT "PK_Appointments" PRIMARY KEY ("Id");


--
-- Name: BlockedTimes PK_BlockedTimes; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public."BlockedTimes"
    ADD CONSTRAINT "PK_BlockedTimes" PRIMARY KEY ("Id");


--
-- Name: Customers PK_Customers; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public."Customers"
    ADD CONSTRAINT "PK_Customers" PRIMARY KEY ("Id");


--
-- Name: Payment PK_Payment; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public."Payment"
    ADD CONSTRAINT "PK_Payment" PRIMARY KEY ("Id");


--
-- Name: Services PK_Services; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public."Services"
    ADD CONSTRAINT "PK_Services" PRIMARY KEY ("Id");


--
-- Name: Staff PK_Staff; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public."Staff"
    ADD CONSTRAINT "PK_Staff" PRIMARY KEY ("Id");


--
-- Name: StaffLunchBreaks PK_StaffLunchBreaks; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public."StaffLunchBreaks"
    ADD CONSTRAINT "PK_StaffLunchBreaks" PRIMARY KEY ("Id");


--
-- Name: StaffServices PK_StaffServices; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public."StaffServices"
    ADD CONSTRAINT "PK_StaffServices" PRIMARY KEY ("StaffId", "ServiceId");


--
-- Name: StaffWorkingHours PK_StaffWorkingHours; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public."StaffWorkingHours"
    ADD CONSTRAINT "PK_StaffWorkingHours" PRIMARY KEY ("Id");


--
-- Name: Tenant PK_Tenant; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public."Tenant"
    ADD CONSTRAINT "PK_Tenant" PRIMARY KEY ("Id");


--
-- Name: TenantWorkingHours PK_TenantWorkingHours; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public."TenantWorkingHours"
    ADD CONSTRAINT "PK_TenantWorkingHours" PRIMARY KEY ("Id");


--
-- Name: User PK_User; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public."User"
    ADD CONSTRAINT "PK_User" PRIMARY KEY ("Id");


--
-- Name: __EFMigrationsHistory PK___EFMigrationsHistory; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public."__EFMigrationsHistory"
    ADD CONSTRAINT "PK___EFMigrationsHistory" PRIMARY KEY ("MigrationId");


--
-- Name: IX_Appointments_BusinessId_StartDateTime; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX "IX_Appointments_BusinessId_StartDateTime" ON public."Appointments" USING btree ("TenantId", "StartDateTime");


--
-- Name: IX_Appointments_CustomerId; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX "IX_Appointments_CustomerId" ON public."Appointments" USING btree ("CustomerId");


--
-- Name: IX_Appointments_ServiceId; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX "IX_Appointments_ServiceId" ON public."Appointments" USING btree ("ServiceId");


--
-- Name: IX_Appointments_StaffId_StartDateTime; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX "IX_Appointments_StaffId_StartDateTime" ON public."Appointments" USING btree ("StaffId", "StartDateTime");


--
-- Name: IX_BlockedTimes_BusinessId; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX "IX_BlockedTimes_BusinessId" ON public."BlockedTimes" USING btree ("TenantId");


--
-- Name: IX_BlockedTimes_StaffId; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX "IX_BlockedTimes_StaffId" ON public."BlockedTimes" USING btree ("StaffId");


--
-- Name: IX_Customers_BusinessId; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX "IX_Customers_BusinessId" ON public."Customers" USING btree ("TenantId");


--
-- Name: IX_Payment_AppointmentId; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX "IX_Payment_AppointmentId" ON public."Payment" USING btree ("AppointmentId");


--
-- Name: IX_Services_BusinessId; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX "IX_Services_BusinessId" ON public."Services" USING btree ("TenantId");


--
-- Name: IX_StaffLunchBreaks_StaffId_DayOfWeek; Type: INDEX; Schema: public; Owner: postgres
--

CREATE UNIQUE INDEX "IX_StaffLunchBreaks_StaffId_DayOfWeek" ON public."StaffLunchBreaks" USING btree ("StaffId", "DayOfWeek");


--
-- Name: IX_StaffLunchBreaks_TenantId; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX "IX_StaffLunchBreaks_TenantId" ON public."StaffLunchBreaks" USING btree ("TenantId");


--
-- Name: IX_StaffServices_ServiceId; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX "IX_StaffServices_ServiceId" ON public."StaffServices" USING btree ("ServiceId");


--
-- Name: IX_StaffWorkingHours_StaffId_DayOfWeek; Type: INDEX; Schema: public; Owner: postgres
--

CREATE UNIQUE INDEX "IX_StaffWorkingHours_StaffId_DayOfWeek" ON public."StaffWorkingHours" USING btree ("StaffId", "DayOfWeek");


--
-- Name: IX_Staff_BusinessId; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX "IX_Staff_BusinessId" ON public."Staff" USING btree ("TenantId");


--
-- Name: IX_Staff_UserId; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX "IX_Staff_UserId" ON public."Staff" USING btree ("UserId");


--
-- Name: IX_Tenant_Slug; Type: INDEX; Schema: public; Owner: postgres
--

CREATE UNIQUE INDEX "IX_Tenant_Slug" ON public."Tenant" USING btree ("Slug");


--
-- Name: IX_User_BusinessId; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX "IX_User_BusinessId" ON public."User" USING btree ("TenantId");


--
-- Name: Appointments FK_Appointments_Customers_CustomerId; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public."Appointments"
    ADD CONSTRAINT "FK_Appointments_Customers_CustomerId" FOREIGN KEY ("CustomerId") REFERENCES public."Customers"("Id") ON DELETE CASCADE;


--
-- Name: Appointments FK_Appointments_Services_ServiceId; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public."Appointments"
    ADD CONSTRAINT "FK_Appointments_Services_ServiceId" FOREIGN KEY ("ServiceId") REFERENCES public."Services"("Id") ON DELETE CASCADE;


--
-- Name: Appointments FK_Appointments_Staff_StaffId; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public."Appointments"
    ADD CONSTRAINT "FK_Appointments_Staff_StaffId" FOREIGN KEY ("StaffId") REFERENCES public."Staff"("Id") ON DELETE CASCADE;


--
-- Name: Appointments FK_Appointments_Tenant_BusinessId; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public."Appointments"
    ADD CONSTRAINT "FK_Appointments_Tenant_BusinessId" FOREIGN KEY ("TenantId") REFERENCES public."Tenant"("Id") ON DELETE CASCADE;


--
-- Name: BlockedTimes FK_BlockedTimes_Staff_StaffId; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public."BlockedTimes"
    ADD CONSTRAINT "FK_BlockedTimes_Staff_StaffId" FOREIGN KEY ("StaffId") REFERENCES public."Staff"("Id");


--
-- Name: BlockedTimes FK_BlockedTimes_Tenant_BusinessId; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public."BlockedTimes"
    ADD CONSTRAINT "FK_BlockedTimes_Tenant_BusinessId" FOREIGN KEY ("TenantId") REFERENCES public."Tenant"("Id") ON DELETE CASCADE;


--
-- Name: Customers FK_Customers_Tenant_BusinessId; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public."Customers"
    ADD CONSTRAINT "FK_Customers_Tenant_BusinessId" FOREIGN KEY ("TenantId") REFERENCES public."Tenant"("Id") ON DELETE CASCADE;


--
-- Name: Payment FK_Payment_Appointments_AppointmentId; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public."Payment"
    ADD CONSTRAINT "FK_Payment_Appointments_AppointmentId" FOREIGN KEY ("AppointmentId") REFERENCES public."Appointments"("Id") ON DELETE CASCADE;


--
-- Name: Services FK_Services_Tenant_BusinessId; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public."Services"
    ADD CONSTRAINT "FK_Services_Tenant_BusinessId" FOREIGN KEY ("TenantId") REFERENCES public."Tenant"("Id") ON DELETE CASCADE;


--
-- Name: StaffLunchBreaks FK_StaffLunchBreaks_Staff_StaffId; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public."StaffLunchBreaks"
    ADD CONSTRAINT "FK_StaffLunchBreaks_Staff_StaffId" FOREIGN KEY ("StaffId") REFERENCES public."Staff"("Id") ON DELETE CASCADE;


--
-- Name: StaffServices FK_StaffServices_Services_ServiceId; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public."StaffServices"
    ADD CONSTRAINT "FK_StaffServices_Services_ServiceId" FOREIGN KEY ("ServiceId") REFERENCES public."Services"("Id") ON DELETE CASCADE;


--
-- Name: StaffServices FK_StaffServices_Staff_StaffId; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public."StaffServices"
    ADD CONSTRAINT "FK_StaffServices_Staff_StaffId" FOREIGN KEY ("StaffId") REFERENCES public."Staff"("Id") ON DELETE CASCADE;


--
-- Name: Staff FK_Staff_Tenant_BusinessId; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public."Staff"
    ADD CONSTRAINT "FK_Staff_Tenant_BusinessId" FOREIGN KEY ("TenantId") REFERENCES public."Tenant"("Id") ON DELETE CASCADE;


--
-- Name: Staff FK_Staff_User_UserId; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public."Staff"
    ADD CONSTRAINT "FK_Staff_User_UserId" FOREIGN KEY ("UserId") REFERENCES public."User"("Id");


--
-- Name: User FK_User_Tenant_BusinessId; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public."User"
    ADD CONSTRAINT "FK_User_Tenant_BusinessId" FOREIGN KEY ("TenantId") REFERENCES public."Tenant"("Id");


--
-- PostgreSQL database dump complete
--

\unrestrict NBINFb3xNj7UfU3p4odrVD6aaOzijfBOjtf9zwAlzzqhB8GssfkpYMVaggWe1EC

