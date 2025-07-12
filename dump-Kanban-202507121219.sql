--
-- PostgreSQL database dump
--

-- Dumped from database version 16.2
-- Dumped by pg_dump version 16.2

-- Started on 2025-07-12 12:19:23

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
-- TOC entry 4 (class 2615 OID 2200)
-- Name: public; Type: SCHEMA; Schema: -; Owner: pg_database_owner
--

CREATE SCHEMA public;


ALTER SCHEMA public OWNER TO pg_database_owner;

--
-- TOC entry 4876 (class 0 OID 0)
-- Dependencies: 4
-- Name: SCHEMA public; Type: COMMENT; Schema: -; Owner: pg_database_owner
--

COMMENT ON SCHEMA public IS 'standard public schema';


--
-- TOC entry 845 (class 1247 OID 42903)
-- Name: changetypeenum; Type: TYPE; Schema: public; Owner: postgres
--

CREATE TYPE public.changetypeenum AS ENUM (
    'StatusChange',
    'AssignmentChange',
    'Creation'
);


ALTER TYPE public.changetypeenum OWNER TO postgres;

--
-- TOC entry 848 (class 1247 OID 42910)
-- Name: taskstatus; Type: TYPE; Schema: public; Owner: postgres
--

CREATE TYPE public.taskstatus AS ENUM (
    'ToDo',
    'InProgress',
    'Done'
);


ALTER TYPE public.taskstatus OWNER TO postgres;

SET default_tablespace = '';

SET default_table_access_method = heap;

--
-- TOC entry 215 (class 1259 OID 42917)
-- Name: Users; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public."Users" (
    "Id" integer NOT NULL,
    "Name" character varying(100) NOT NULL,
    "Email" character varying(255) NOT NULL,
    "CreatedAt" timestamp without time zone DEFAULT CURRENT_TIMESTAMP
);


ALTER TABLE public."Users" OWNER TO postgres;

--
-- TOC entry 216 (class 1259 OID 42921)
-- Name: Users_Id_seq; Type: SEQUENCE; Schema: public; Owner: postgres
--

CREATE SEQUENCE public."Users_Id_seq"
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER SEQUENCE public."Users_Id_seq" OWNER TO postgres;

--
-- TOC entry 4877 (class 0 OID 0)
-- Dependencies: 216
-- Name: Users_Id_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: postgres
--

ALTER SEQUENCE public."Users_Id_seq" OWNED BY public."Users"."Id";


--
-- TOC entry 217 (class 1259 OID 42922)
-- Name: taskhistories; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public.taskhistories (
    id integer NOT NULL,
    taskid integer NOT NULL,
    changedbyuserid integer NOT NULL,
    changetype character varying NOT NULL,
    oldvalue text,
    newvalue text,
    changedate timestamp without time zone DEFAULT CURRENT_TIMESTAMP NOT NULL
);


ALTER TABLE public.taskhistories OWNER TO postgres;

--
-- TOC entry 218 (class 1259 OID 42928)
-- Name: taskhistories_id_seq; Type: SEQUENCE; Schema: public; Owner: postgres
--

CREATE SEQUENCE public.taskhistories_id_seq
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER SEQUENCE public.taskhistories_id_seq OWNER TO postgres;

--
-- TOC entry 4878 (class 0 OID 0)
-- Dependencies: 218
-- Name: taskhistories_id_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: postgres
--

ALTER SEQUENCE public.taskhistories_id_seq OWNED BY public.taskhistories.id;


--
-- TOC entry 219 (class 1259 OID 42929)
-- Name: tasks; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public.tasks (
    id integer NOT NULL,
    title character varying(255) NOT NULL,
    description text,
    status integer NOT NULL,
    assignedtouserid integer,
    createdat timestamp without time zone DEFAULT CURRENT_TIMESTAMP NOT NULL,
    updatedat timestamp without time zone DEFAULT CURRENT_TIMESTAMP NOT NULL
);


ALTER TABLE public.tasks OWNER TO postgres;

--
-- TOC entry 220 (class 1259 OID 42936)
-- Name: tasks_id_seq; Type: SEQUENCE; Schema: public; Owner: postgres
--

CREATE SEQUENCE public.tasks_id_seq
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER SEQUENCE public.tasks_id_seq OWNER TO postgres;

--
-- TOC entry 4879 (class 0 OID 0)
-- Dependencies: 220
-- Name: tasks_id_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: postgres
--

ALTER SEQUENCE public.tasks_id_seq OWNED BY public.tasks.id;


--
-- TOC entry 4704 (class 2604 OID 42937)
-- Name: Users Id; Type: DEFAULT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public."Users" ALTER COLUMN "Id" SET DEFAULT nextval('public."Users_Id_seq"'::regclass);


--
-- TOC entry 4706 (class 2604 OID 42938)
-- Name: taskhistories id; Type: DEFAULT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.taskhistories ALTER COLUMN id SET DEFAULT nextval('public.taskhistories_id_seq'::regclass);


--
-- TOC entry 4708 (class 2604 OID 42939)
-- Name: tasks id; Type: DEFAULT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.tasks ALTER COLUMN id SET DEFAULT nextval('public.tasks_id_seq'::regclass);


--
-- TOC entry 4865 (class 0 OID 42917)
-- Dependencies: 215
-- Data for Name: Users; Type: TABLE DATA; Schema: public; Owner: postgres
--

COPY public."Users" ("Id", "Name", "Email", "CreatedAt") FROM stdin;
1	testB	testB	2025-07-12 09:41:09.578773
2	testA	testA	2025-07-12 10:54:45.331244
\.


--
-- TOC entry 4867 (class 0 OID 42922)
-- Dependencies: 217
-- Data for Name: taskhistories; Type: TABLE DATA; Schema: public; Owner: postgres
--

COPY public.taskhistories (id, taskid, changedbyuserid, changetype, oldvalue, newvalue, changedate) FROM stdin;
\.


--
-- TOC entry 4869 (class 0 OID 42929)
-- Dependencies: 219
-- Data for Name: tasks; Type: TABLE DATA; Schema: public; Owner: postgres
--

COPY public.tasks (id, title, description, status, assignedtouserid, createdat, updatedat) FROM stdin;
\.


--
-- TOC entry 4880 (class 0 OID 0)
-- Dependencies: 216
-- Name: Users_Id_seq; Type: SEQUENCE SET; Schema: public; Owner: postgres
--

SELECT pg_catalog.setval('public."Users_Id_seq"', 2, true);


--
-- TOC entry 4881 (class 0 OID 0)
-- Dependencies: 218
-- Name: taskhistories_id_seq; Type: SEQUENCE SET; Schema: public; Owner: postgres
--

SELECT pg_catalog.setval('public.taskhistories_id_seq', 13, true);


--
-- TOC entry 4882 (class 0 OID 0)
-- Dependencies: 220
-- Name: tasks_id_seq; Type: SEQUENCE SET; Schema: public; Owner: postgres
--

SELECT pg_catalog.setval('public.tasks_id_seq', 8, true);


--
-- TOC entry 4712 (class 2606 OID 42941)
-- Name: Users Users_Email_key; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public."Users"
    ADD CONSTRAINT "Users_Email_key" UNIQUE ("Email");


--
-- TOC entry 4714 (class 2606 OID 42943)
-- Name: Users Users_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public."Users"
    ADD CONSTRAINT "Users_pkey" PRIMARY KEY ("Id");


--
-- TOC entry 4716 (class 2606 OID 42945)
-- Name: taskhistories taskhistories_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.taskhistories
    ADD CONSTRAINT taskhistories_pkey PRIMARY KEY (id);


--
-- TOC entry 4718 (class 2606 OID 42947)
-- Name: tasks tasks_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.tasks
    ADD CONSTRAINT tasks_pkey PRIMARY KEY (id);


--
-- TOC entry 4719 (class 2606 OID 42948)
-- Name: taskhistories fk_history_task; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.taskhistories
    ADD CONSTRAINT fk_history_task FOREIGN KEY (taskid) REFERENCES public.tasks(id) ON DELETE CASCADE;


--
-- TOC entry 4720 (class 2606 OID 42953)
-- Name: taskhistories fk_history_user; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.taskhistories
    ADD CONSTRAINT fk_history_user FOREIGN KEY (changedbyuserid) REFERENCES public."Users"("Id") ON DELETE RESTRICT;


--
-- TOC entry 4721 (class 2606 OID 42958)
-- Name: tasks fk_task_user; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.tasks
    ADD CONSTRAINT fk_task_user FOREIGN KEY (assignedtouserid) REFERENCES public."Users"("Id") ON DELETE SET NULL;


-- Completed on 2025-07-12 12:19:23

--
-- PostgreSQL database dump complete
--

