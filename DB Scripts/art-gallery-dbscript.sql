CREATE DATABASE ARTGALLERY;

CREATE TABLE public.state (
    state_id INT NOT NULL GENERATED ALWAYS AS IDENTITY,
    name VARCHAR(50) NOT NULL,
    num_sub_groups INT NOT NULL,
    num_languages INT NOT NULL,
    created_date TIMESTAMP NOT NULL,
    modified_date TIMESTAMP NOT NULL,
    PRIMARY KEY (state_id)
);

CREATE TABLE public.artist (
    artist_id INT NOT NULL GENERATED ALWAYS AS IDENTITY,
    name VARCHAR(100) NOT NULL,
    description VARCHAR(800) NULL,
    age INT NULL,
    state_id INT NOT NULL,
    language VARCHAR(100) NOT NULL,
    sub_group VARCHAR(100) NOT NULL,
    created_date TIMESTAMP NOT NULL,
    modified_date TIMESTAMP NOT NULL,
    PRIMARY KEY (artist_id),
    CONSTRAINT fk_state
        FOREIGN KEY (state_id)
            REFERENCES public.state(state_id)
);

CREATE TABLE public.artefact (
    artefact_id INT NOT NULL GENERATED ALWAYS AS IDENTITY,
    artist_id INT NOT NULL,
    title VARCHAR(100) NOT NULL,
    description VARCHAR(800) NULL,
    medium VARCHAR(50) NOT NULL,
    year INT NOT NULL,
    height_cm DECIMAL NULL,
    width_cm DECIMAL NULL,
    img_url VARCHAR(100) NOT NULL,
    created_date TIMESTAMP NOT NULL,
    modified_date TIMESTAMP NOT NULL,
    PRIMARY KEY (artefact_id),
    CONSTRAINT fk_artist
        FOREIGN KEY (artist_id)
            REFERENCES public.artist(artist_id)
);