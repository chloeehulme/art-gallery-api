CREATE TABLE public.state (
    id INT NOT NULL GENERATED ALWAYS AS IDENTITY,
    name VARCHAR(50) NOT NULL,
    language_groups INT NOT NULL,
    created_date TIMESTAMP NOT NULL,
    modified_date TIMESTAMP NOT NULL,
    PRIMARY KEY (id)
);

CREATE TABLE public.artist (
    id INT NOT NULL GENERATED ALWAYS AS IDENTITY,
    name VARCHAR(100) NOT NULL,
    description VARCHAR(800) NULL,
    age INT NOT NULL,
    state_id INT NOT NULL,
    language_group VARCHAR(100) NOT NULL,
    created_date TIMESTAMP NOT NULL,
    modified_date TIMESTAMP NOT NULL,
    PRIMARY KEY (id),
    CONSTRAINT fk_state
        FOREIGN KEY (state_id)
            REFERENCES public.state(id)
);

CREATE TABLE public.artefact (
    id INT NOT NULL GENERATED ALWAYS AS IDENTITY,
    artist_id INT NOT NULL,
    title VARCHAR(100) NOT NULL,
    description VARCHAR(800) NULL,
    medium VARCHAR(50) NOT NULL,
    year INT NOT NULL,
    height_cm DECIMAL NOT NULL,
    width_cm DECIMAL NOT NULL,
    img_url VARCHAR(200) NULL,
    created_date TIMESTAMP NOT NULL,
    modified_date TIMESTAMP NOT NULL,
    PRIMARY KEY (id),
    CONSTRAINT fk_artist
        FOREIGN KEY (artist_id)
            REFERENCES public.artist(id)
);

CREATE TABLE public.user (
    id INT NOT NULL GENERATED ALWAYS AS IDENTITY,
    email VARCHAR(200) NOT NULL,
    first_name VARCHAR(50) NOT NULL,
    last_name VARCHAR(50) NOT NULL,
    password_hash VARCHAR(300) NOT NULL,
    description VARCHAR(800) NULL,
    role VARCHAR(800) NULL,
    created_date TIMESTAMP NOT NULL,
    modified_date TIMESTAMP NOT NULL,
    PRIMARY KEY (id)
);

INSERT INTO public.state VALUES(DEFAULT, 'Victoria', 38, CURRENT_TIMESTAMP, CURRENT_TIMESTAMP);
INSERT INTO public.state VALUES(DEFAULT, 'New South Wales', 29, CURRENT_TIMESTAMP, CURRENT_TIMESTAMP);
INSERT INTO public.state VALUES(DEFAULT, 'Queensland', 50, CURRENT_TIMESTAMP, CURRENT_TIMESTAMP);
INSERT INTO public.state VALUES(DEFAULT, 'South Australia', 46, CURRENT_TIMESTAMP, CURRENT_TIMESTAMP);
INSERT INTO public.state VALUES(DEFAULT, 'Western Australia', 90, CURRENT_TIMESTAMP, CURRENT_TIMESTAMP);
INSERT INTO public.state VALUES(DEFAULT, 'Northern Territory', 100, CURRENT_TIMESTAMP, CURRENT_TIMESTAMP);
INSERT INTO public.state VALUES(DEFAULT, 'Australian Capital Territory', 1, CURRENT_TIMESTAMP, CURRENT_TIMESTAMP);
INSERT INTO public.state VALUES(DEFAULT, 'Tasmania', 8, CURRENT_TIMESTAMP, CURRENT_TIMESTAMP);

INSERT INTO public.artist VALUES(DEFAULT, 'Betty Muffler', 'Betty Muffler is an Aboriginal Australian artist and ngangkari (healer). She is a senior artist at Iwantja Arts, in Indulkana in Aṉangu Pitjantjatjara Yankunytjatjara (APY Lands), South Australia, known for a series of works on large linen canvases called Ngangkari Ngura (Healing Country).', 78, 4, 'Yankunytjatjara', CURRENT_TIMESTAMP, CURRENT_TIMESTAMP);
INSERT INTO public.artist VALUES(DEFAULT, 'Emily Kame Kngwarreye', 'Emily Kame Kngwarreye is one of Australia’s most significant contemporary artists. Emily was born at the beginning of the 20th century and grew up in a remote desert area known as Utopia, 230 kilometres north-east of Alice Springs, distant from the art world that sought her work.', 85, 6, 'Arrernte', CURRENT_TIMESTAMP, CURRENT_TIMESTAMP);
INSERT INTO public.artist VALUES(DEFAULT, 'Tony Albert', 'Tony Albert (born 1981) is a contemporary Australian artist working in a wide range of mediums including painting, photography and mixed media. His work engages with political, historical and cultural Aboriginal and Australian history, and his fascination with kitsch "Aboriginalia".', 41, 3, 'Guugu Yalandji', CURRENT_TIMESTAMP, CURRENT_TIMESTAMP);

INSERT INTO public.artefact VALUES(DEFAULT, 3, 'Walkabout 2016', 'Mid century modern.', 'Pigment print on paper.', 2016, 50, 50, 'https://tonyalbert.com.au/wp-content/uploads/2014/11/TONY-ALBERT-Mid-Century-Modern-Walkabout-2016-Pigment-print-on-paper-50-x-50-cm.jpg', CURRENT_TIMESTAMP, CURRENT_TIMESTAMP);
INSERT INTO public.artefact VALUES(DEFAULT, 3, 'Crop Circles In Yogya #1', 'Unique edition.', 'Pigment print on paper with hand embellishment.', 2016, 83, 113, 'https://tonyalbert.com.au/wp-content/uploads/2017/02/Crop-Circles-in-Yogya-1.-2016-unique-edition-pigment-print-on-paper-with-hand-embellishment-83-x-113-cm-detail_1-Tony-Albert.jpg', CURRENT_TIMESTAMP, CURRENT_TIMESTAMP);
INSERT INTO public.artefact VALUES(DEFAULT, 3, 'Thou Didst Let Fall', NULL, 'Objects, fabric and twine.', 2014, 161, 550, 'https://tonyalbert.com.au/wp-content/uploads/2015/07/Tony-Albert-Thou-didst-let-fall-2014-objects-fabrics-and-twine-161-x-550-x-11-cm.jpg', CURRENT_TIMESTAMP, CURRENT_TIMESTAMP);
INSERT INTO public.artefact VALUES(DEFAULT, 2, 'Arlatyeye', 'Arlatyeye is an Indigenous Australian Art acrylic painting created by Emily Kame Kngwarreye in 1995. It lives at the National Gallery of Australia in Australia. The image is used according to educational fair use, and tagged abstract art.', 'Acrylic on canvas.', 1995, 121, 91, 'https://arthistoryproject.com/site/assets/files/31127/emily_kame_kngwarreye-arlatyeye-1995-trivium-art-history-1.jpg', CURRENT_TIMESTAMP, CURRENT_TIMESTAMP);
INSERT INTO public.artefact VALUES(DEFAULT, 2, 'Untitled, 1992', 'Untitled, 1992 is an Indigenous Australian Art acrylic painting created by Emily Kame Kngwarreye in 1992. The image is used according to educational fair use, and tagged abstract art.', 'Acrylic on canvas.', 1992, 121.3, 91, 'https://arthistoryproject.com/site/assets/files/31096/emily_kame_kngwarreye-untitled-_1992-1992-trivium-art-history-1.jpg', CURRENT_TIMESTAMP, CURRENT_TIMESTAMP);
INSERT INTO public.artefact VALUES(DEFAULT, 2, 'Alhalkere, My Country', 'Alhalkere, My Country is an Indigenous Australian Art acrylic painting created by Emily Kame Kngwarreye in 1994. The image is used according to educational fair use, and tagged abstract art and landscape painting.', 'Acrylic on canvas.', 1994, 152, 488, 'https://arthistoryproject.com/site/assets/files/31143/emily_kame_kngwarreye-alhalkere-_my_country-1994-trivium-art-history-1.jpg', CURRENT_TIMESTAMP, CURRENT_TIMESTAMP);
INSERT INTO public.artefact VALUES(DEFAULT, 1, 'Ngangkaṟi Ngura (Healing Country) 884-21', 'Winner, Best General Painting Award, National Aboriginal and Torres Strait Islander Award 2022.', 'Acrylic on linen.', 2021, 167, 198, 'https://www.janmurphygallery.com.au/wp-content/uploads/2022/08/MUF12695.jpg', CURRENT_TIMESTAMP, CURRENT_TIMESTAMP);
INSERT INTO public.artefact VALUES(DEFAULT, 1, 'Ngangkari Ngura (Healing Country) 723-21', NULL, 'Acrylic on linen.', 2021, 152, 122, 'https://www.janmurphygallery.com.au/wp-content/uploads/2022/06/MUF12083_1.jpg', CURRENT_TIMESTAMP, CURRENT_TIMESTAMP);
INSERT INTO public.artefact VALUES(DEFAULT, 1, 'Ngangkari Ngura (Healing Country) 700-19', 'Finalist Wynne Prize 2020.', 'Acrylic on linen.', 2020, 152, 198, 'https://www.janmurphygallery.com.au/wp-content/uploads/2022/08/MUF12600_1.jpg', CURRENT_TIMESTAMP, CURRENT_TIMESTAMP);


CREATE FUNCTION get_artefact_count(artefact_year_from int, artefact_year_to int)
returns int
LANGUAGE plpgsql
as
$$
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


CREATE FUNCTION get_artist_count_by_state(state_id_param int)
returns int
LANGUAGE plpgsql
as
$$
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