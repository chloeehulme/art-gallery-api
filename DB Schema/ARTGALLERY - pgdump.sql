--
-- PostgreSQL database dump
--

-- Dumped from database version 14.4
-- Dumped by pg_dump version 14.4

-- Started on 2022-10-03 21:55:19 AEDT

SET statement_timeout = 0;
SET lock_timeout = 0;
SET idle_in_transaction_session_timeout = 0;
SET client_encoding = 'UTF8';
SET standard_conforming_strings = on;
SELECT pg_catalog.set_config('search_path', '', false);
SET check_function_bodies = false;
SET xmloption = content;
SET client_min_messages = warning;
SET row_security = off;

--
-- TOC entry 217 (class 1255 OID 32976)
-- Name: get_artefact_count(integer, integer); Type: FUNCTION; Schema: public; Owner: postgres
--

CREATE FUNCTION public.get_artefact_count(artefact_year_from integer, artefact_year_to integer) RETURNS integer
    LANGUAGE plpgsql
    AS $$
declare
    artefact_count INTEGER;
begin 
    SELECT count(*)
    into artefact_count
    from public.artefact
    where year between artefact_year_from and artefact_year_to;

    return artefact_count;
end;
$$;


ALTER FUNCTION public.get_artefact_count(artefact_year_from integer, artefact_year_to integer) OWNER TO postgres;

--
-- TOC entry 218 (class 1255 OID 32977)
-- Name: get_artist_count_by_state(integer); Type: FUNCTION; Schema: public; Owner: postgres
--

CREATE FUNCTION public.get_artist_count_by_state(state_id_param integer) RETURNS integer
    LANGUAGE plpgsql
    AS $$
declare
    artist_count INTEGER;
begin 
    SELECT count(*)
    into artist_count
    from public.artist
    where state_id=state_id_param;

    return artist_count;
end;
$$;


ALTER FUNCTION public.get_artist_count_by_state(state_id_param integer) OWNER TO postgres;

SET default_tablespace = '';

SET default_table_access_method = heap;

--
-- TOC entry 214 (class 1259 OID 32956)
-- Name: artefact; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public.artefact (
    id integer NOT NULL,
    artist_id integer NOT NULL,
    title character varying(100) NOT NULL,
    description character varying(800),
    medium character varying(50) NOT NULL,
    year integer NOT NULL,
    height_cm numeric NOT NULL,
    width_cm numeric NOT NULL,
    img_url character varying(200),
    created_date timestamp without time zone NOT NULL,
    modified_date timestamp without time zone NOT NULL
);


ALTER TABLE public.artefact OWNER TO postgres;

--
-- TOC entry 213 (class 1259 OID 32955)
-- Name: artefact_id_seq; Type: SEQUENCE; Schema: public; Owner: postgres
--

ALTER TABLE public.artefact ALTER COLUMN id ADD GENERATED ALWAYS AS IDENTITY (
    SEQUENCE NAME public.artefact_id_seq
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1
);


--
-- TOC entry 212 (class 1259 OID 32943)
-- Name: artist; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public.artist (
    id integer NOT NULL,
    name character varying(100) NOT NULL,
    description character varying(800),
    age integer NOT NULL,
    state_id integer NOT NULL,
    language_group character varying(100) NOT NULL,
    created_date timestamp without time zone NOT NULL,
    modified_date timestamp without time zone NOT NULL
);


ALTER TABLE public.artist OWNER TO postgres;

--
-- TOC entry 211 (class 1259 OID 32942)
-- Name: artist_id_seq; Type: SEQUENCE; Schema: public; Owner: postgres
--

ALTER TABLE public.artist ALTER COLUMN id ADD GENERATED ALWAYS AS IDENTITY (
    SEQUENCE NAME public.artist_id_seq
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1
);


--
-- TOC entry 210 (class 1259 OID 32937)
-- Name: state; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public.state (
    id integer NOT NULL,
    name character varying(50) NOT NULL,
    language_groups integer NOT NULL,
    created_date timestamp without time zone NOT NULL,
    modified_date timestamp without time zone NOT NULL
);


ALTER TABLE public.state OWNER TO postgres;

--
-- TOC entry 209 (class 1259 OID 32936)
-- Name: state_id_seq; Type: SEQUENCE; Schema: public; Owner: postgres
--

ALTER TABLE public.state ALTER COLUMN id ADD GENERATED ALWAYS AS IDENTITY (
    SEQUENCE NAME public.state_id_seq
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1
);


--
-- TOC entry 216 (class 1259 OID 32969)
-- Name: user; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public."user" (
    id integer NOT NULL,
    email character varying(200) NOT NULL,
    first_name character varying(50) NOT NULL,
    last_name character varying(50) NOT NULL,
    password_hash character varying(300) NOT NULL,
    description character varying(800),
    role character varying(800),
    created_date timestamp without time zone NOT NULL,
    modified_date timestamp without time zone NOT NULL
);


ALTER TABLE public."user" OWNER TO postgres;

--
-- TOC entry 215 (class 1259 OID 32968)
-- Name: user_id_seq; Type: SEQUENCE; Schema: public; Owner: postgres
--

ALTER TABLE public."user" ALTER COLUMN id ADD GENERATED ALWAYS AS IDENTITY (
    SEQUENCE NAME public.user_id_seq
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1
);


--
-- TOC entry 3601 (class 0 OID 32956)
-- Dependencies: 214
-- Data for Name: artefact; Type: TABLE DATA; Schema: public; Owner: postgres
--

COPY public.artefact (id, artist_id, title, description, medium, year, height_cm, width_cm, img_url, created_date, modified_date) FROM stdin;
1	3	Walkabout 2016	Mid century modern.	Pigment print on paper.	2016	50	50	https://tonyalbert.com.au/wp-content/uploads/2014/11/TONY-ALBERT-Mid-Century-Modern-Walkabout-2016-Pigment-print-on-paper-50-x-50-cm.jpg	2022-09-27 19:07:59.444762	2022-09-27 19:07:59.444762
2	3	Crop Circles In Yogya #1	Unique edition.	Pigment print on paper with hand embellishment.	2016	83	113	https://tonyalbert.com.au/wp-content/uploads/2017/02/Crop-Circles-in-Yogya-1.-2016-unique-edition-pigment-print-on-paper-with-hand-embellishment-83-x-113-cm-detail_1-Tony-Albert.jpg	2022-09-27 19:07:59.451315	2022-09-27 19:07:59.451315
3	3	Thou Didst Let Fall	\N	Objects, fabric and twine.	2014	161	550	https://tonyalbert.com.au/wp-content/uploads/2015/07/Tony-Albert-Thou-didst-let-fall-2014-objects-fabrics-and-twine-161-x-550-x-11-cm.jpg	2022-09-27 19:07:59.452209	2022-09-27 19:07:59.452209
4	2	Arlatyeye	Arlatyeye is an Indigenous Australian Art acrylic painting created by Emily Kame Kngwarreye in 1995. It lives at the National Gallery of Australia in Australia. The image is used according to educational fair use, and tagged abstract art.	Acrylic on canvas.	1995	121	91	https://arthistoryproject.com/site/assets/files/31127/emily_kame_kngwarreye-arlatyeye-1995-trivium-art-history-1.jpg	2022-09-27 19:07:59.452691	2022-09-27 19:07:59.452691
5	2	Untitled, 1992	Untitled, 1992 is an Indigenous Australian Art acrylic painting created by Emily Kame Kngwarreye in 1992. The image is used according to educational fair use, and tagged abstract art.	Acrylic on canvas.	1992	121.3	91	https://arthistoryproject.com/site/assets/files/31096/emily_kame_kngwarreye-untitled-_1992-1992-trivium-art-history-1.jpg	2022-09-27 19:07:59.453089	2022-09-27 19:07:59.453089
6	2	Alhalkere, My Country	Alhalkere, My Country is an Indigenous Australian Art acrylic painting created by Emily Kame Kngwarreye in 1994. The image is used according to educational fair use, and tagged abstract art and landscape painting.	Acrylic on canvas.	1994	152	488	https://arthistoryproject.com/site/assets/files/31143/emily_kame_kngwarreye-alhalkere-_my_country-1994-trivium-art-history-1.jpg	2022-09-27 19:07:59.453429	2022-09-27 19:07:59.453429
7	1	Ngangkaṟi Ngura (Healing Country) 884-21	Winner, Best General Painting Award, National Aboriginal and Torres Strait Islander Award 2022.	Acrylic on linen.	2021	167	198	https://www.janmurphygallery.com.au/wp-content/uploads/2022/08/MUF12695.jpg	2022-09-27 19:07:59.454001	2022-09-27 19:07:59.454001
8	1	Ngangkari Ngura (Healing Country) 723-21	\N	Acrylic on linen.	2021	152	122	https://www.janmurphygallery.com.au/wp-content/uploads/2022/06/MUF12083_1.jpg	2022-09-27 19:07:59.45437	2022-09-27 19:07:59.45437
9	1	Ngangkari Ngura (Healing Country) 700-19	Finalist Wynne Prize 2020.	Acrylic on linen.	2020	152	198	https://www.janmurphygallery.com.au/wp-content/uploads/2022/08/MUF12600_1.jpg	2022-09-27 19:07:59.455147	2022-09-27 19:07:59.455147
\.


--
-- TOC entry 3599 (class 0 OID 32943)
-- Dependencies: 212
-- Data for Name: artist; Type: TABLE DATA; Schema: public; Owner: postgres
--

COPY public.artist (id, name, description, age, state_id, language_group, created_date, modified_date) FROM stdin;
1	Betty Muffler	Betty Muffler is an Aboriginal Australian artist and ngangkari (healer). She is a senior artist at Iwantja Arts, in Indulkana in Aṉangu Pitjantjatjara Yankunytjatjara (APY Lands), South Australia, known for a series of works on large linen canvases called Ngangkari Ngura (Healing Country).	78	4	Yankunytjatjara	2022-09-27 19:07:59.427657	2022-09-27 19:07:59.427657
2	Emily Kame Kngwarreye	Emily Kame Kngwarreye is one of Australia’s most significant contemporary artists. Emily was born at the beginning of the 20th century and grew up in a remote desert area known as Utopia, 230 kilometres north-east of Alice Springs, distant from the art world that sought her work.	85	6	Arrernte	2022-09-27 19:07:59.443517	2022-09-27 19:07:59.443517
3	Tony Albert	Tony Albert (born 1981) is a contemporary Australian artist working in a wide range of mediums including painting, photography and mixed media. His work engages with political, historical and cultural Aboriginal and Australian history, and his fascination with kitsch "Aboriginalia".	41	3	Guugu Yalandji	2022-09-27 19:07:59.444175	2022-09-27 19:07:59.444175
\.


--
-- TOC entry 3597 (class 0 OID 32937)
-- Dependencies: 210
-- Data for Name: state; Type: TABLE DATA; Schema: public; Owner: postgres
--

COPY public.state (id, name, language_groups, created_date, modified_date) FROM stdin;
1	Victoria	38	2022-09-27 19:07:59.409213	2022-09-27 19:07:59.409213
2	New South Wales	29	2022-09-27 19:07:59.424267	2022-09-27 19:07:59.424267
3	Queensland	50	2022-09-27 19:07:59.424804	2022-09-27 19:07:59.424804
4	South Australia	46	2022-09-27 19:07:59.425204	2022-09-27 19:07:59.425204
5	Western Australia	90	2022-09-27 19:07:59.425722	2022-09-27 19:07:59.425722
6	Northern Territory	100	2022-09-27 19:07:59.426241	2022-09-27 19:07:59.426241
7	Australian Capital Territory	1	2022-09-27 19:07:59.426762	2022-09-27 19:07:59.426762
8	Tasmania	8	2022-09-27 19:07:59.427102	2022-09-27 19:07:59.427102
\.


--
-- TOC entry 3603 (class 0 OID 32969)
-- Dependencies: 216
-- Data for Name: user; Type: TABLE DATA; Schema: public; Owner: postgres
--

COPY public."user" (id, email, first_name, last_name, password_hash, description, role, created_date, modified_date) FROM stdin;
1	chloeehulme@gmail.com	Chloe	Hulme	AQAAAAEAACcQAAAAEGC1ncZubP6XJemUKKX+Dtoy5eJjRAoU0TajWlK5wkS2oAdv/l2GxSOaEbzbBXBDlA==	developer	admin	2022-09-30 14:23:00.462559	2022-09-30 14:23:00.462598
\.


--
-- TOC entry 3609 (class 0 OID 0)
-- Dependencies: 213
-- Name: artefact_id_seq; Type: SEQUENCE SET; Schema: public; Owner: postgres
--

SELECT pg_catalog.setval('public.artefact_id_seq', 10, true);


--
-- TOC entry 3610 (class 0 OID 0)
-- Dependencies: 211
-- Name: artist_id_seq; Type: SEQUENCE SET; Schema: public; Owner: postgres
--

SELECT pg_catalog.setval('public.artist_id_seq', 5, true);


--
-- TOC entry 3611 (class 0 OID 0)
-- Dependencies: 209
-- Name: state_id_seq; Type: SEQUENCE SET; Schema: public; Owner: postgres
--

SELECT pg_catalog.setval('public.state_id_seq', 8, true);


--
-- TOC entry 3612 (class 0 OID 0)
-- Dependencies: 215
-- Name: user_id_seq; Type: SEQUENCE SET; Schema: public; Owner: postgres
--

SELECT pg_catalog.setval('public.user_id_seq', 1, true);


--
-- TOC entry 3452 (class 2606 OID 32962)
-- Name: artefact artefact_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.artefact
    ADD CONSTRAINT artefact_pkey PRIMARY KEY (id);


--
-- TOC entry 3450 (class 2606 OID 32949)
-- Name: artist artist_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.artist
    ADD CONSTRAINT artist_pkey PRIMARY KEY (id);


--
-- TOC entry 3448 (class 2606 OID 32941)
-- Name: state state_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.state
    ADD CONSTRAINT state_pkey PRIMARY KEY (id);


--
-- TOC entry 3454 (class 2606 OID 32975)
-- Name: user user_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public."user"
    ADD CONSTRAINT user_pkey PRIMARY KEY (id);


--
-- TOC entry 3456 (class 2606 OID 32963)
-- Name: artefact fk_artist; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.artefact
    ADD CONSTRAINT fk_artist FOREIGN KEY (artist_id) REFERENCES public.artist(id);


--
-- TOC entry 3455 (class 2606 OID 32950)
-- Name: artist fk_state; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.artist
    ADD CONSTRAINT fk_state FOREIGN KEY (state_id) REFERENCES public.state(id);


-- Completed on 2022-10-03 21:55:19 AEDT

--
-- PostgreSQL database dump complete
--

