CREATE TABLE public.state (
    id INT NOT NULL GENERATED ALWAYS AS IDENTITY,
    name VARCHAR(50) NOT NULL,
    languagegroups INT NOT NULL,
    createddate TIMESTAMP NOT NULL,
    modifieddate TIMESTAMP NOT NULL,
    PRIMARY KEY (id)
);

CREATE TABLE public.artist (
    id INT NOT NULL GENERATED ALWAYS AS IDENTITY,
    name VARCHAR(100) NOT NULL,
    description VARCHAR(800) NULL,
    age INT NOT NULL,
    stateid INT NOT NULL,
    languagegroup VARCHAR(100) NOT NULL,
    createddate TIMESTAMP NOT NULL,
    modifieddate TIMESTAMP NOT NULL,
    PRIMARY KEY (id),
    CONSTRAINT fk_state
        FOREIGN KEY (stateid)
            REFERENCES public.state(id)
);

CREATE TABLE public.artefact (
    id INT NOT NULL GENERATED ALWAYS AS IDENTITY,
    artistid INT NOT NULL,
    title VARCHAR(100) NOT NULL,
    description VARCHAR(800) NULL,
    medium VARCHAR(50) NOT NULL,
    year INT NOT NULL,
    heightcm DECIMAL NOT NULL,
    widthcm DECIMAL NOT NULL,
    imgurl VARCHAR(200) NULL,
    createddate TIMESTAMP NOT NULL,
    modifieddate TIMESTAMP NOT NULL,
    PRIMARY KEY (id),
    CONSTRAINT fk_artist
        FOREIGN KEY (artistid)
            REFERENCES public.artist(id)
);

CREATE TABLE public.user (
    userid INT NOT NULL GENERATED ALWAYS AS IDENTITY,
    email VARCHAR(200) NOT NULL,
    firstname VARCHAR(50) NOT NULL,
    lastname VARCHAR(50) NOT NULL,
    passwordhash VARCHAR(300) NOT NULL,
    description VARCHAR(800) NULL,
    role VARCHAR(800) NULL,
    createddate TIMESTAMP NOT NULL,
    modifieddate TIMESTAMP NOT NULL,
    PRIMARY KEY (userid)
);

INSERT INTO public.state VALUES(DEFAULT, 'Victoria', 38, CURRENT_TIMESTAMP, CURRENT_TIMESTAMP);
INSERT INTO public.state VALUES(DEFAULT, 'New South Wales', 29, CURRENT_TIMESTAMP, CURRENT_TIMESTAMP);
INSERT INTO public.state VALUES(DEFAULT, 'Queensland', 50, CURRENT_TIMESTAMP, CURRENT_TIMESTAMP);
INSERT INTO public.state VALUES(DEFAULT, 'South Australia', 46, CURRENT_TIMESTAMP, CURRENT_TIMESTAMP);
INSERT INTO public.state VALUES(DEFAULT, 'Western Australia', 90, CURRENT_TIMESTAMP, CURRENT_TIMESTAMP);
INSERT INTO public.state VALUES(DEFAULT, 'Northern Territory', 100, CURRENT_TIMESTAMP, CURRENT_TIMESTAMP);
INSERT INTO public.state VALUES(DEFAULT, 'Australian Capital Territory', 1, CURRENT_TIMESTAMP, CURRENT_TIMESTAMP);
INSERT INTO public.state VALUES(DEFAULT, 'Tasmania', 8, CURRENT_TIMESTAMP, CURRENT_TIMESTAMP);