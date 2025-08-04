DO $$
DECLARE
    -- Declare UUID variables for locations to establish parent-child relationships
    island_location_id UUID := '10000000-0000-0000-0000-000000000001';
    sudurland_location_id UUID := '10000000-0000-0000-0000-000000000002';
    hofudborgarsvaedid_location_id UUID := '10000000-0000-0000-0000-000000000003';
    thorsmork_location_id UUID := '10000000-0000-0000-0000-000000000004';
    landmannalaugar_location_id UUID := '10000000-0000-0000-0000-000000000005';
    reykjavik_location_id UUID := '10000000-0000-0000-0000-000000000006';
    
    -- Placeholder for a created_by (replace with an actual user from your auth.users table if required)
    seed_created_by UUID := '00000000-0000-0000-0000-000000000001';

    -- Declare UUID variables for new race IDs
    laugavegur_race_id UUID := '20000000-0000-0000-0000-000000000001';
    hengill_ultra_race_id UUID := '20000000-0000-0000-0000-000000000002';
    reykjavik_marathon_race_id UUID := '20000000-0000-0000-0000-000000000003';
    esja_ultra_race_id UUID := '20000000-0000-0000-0000-000000000005';
    hvitasunnuhlaup_hauka_race_id UUID := '20000000-0000-0000-0000-000000000006';

    -- Declare UUID variables for new trail IDs (fetched by slug or declared if new)
    esja_ultra_marathon_trail_id UUID;
    esja_ultra_half_marathon_trail_id UUID;
    hengill_ultra_52_trail_id UUID;
    hvitasunnuhlaup_hauka_22_trail_id UUID;
    hvitasunnuhlaup_hauka_17_trail_id UUID;
    hvitasunnuhlaup_hauka_14_trail_id UUID;
    puffin_run_trail_id UUID;
    bakgardur_ellidavatn_trail_id UUID;
    skaftafell_ultra_trail_id UUID;     
    hafnarfjall_ultra_seven_peaks_and_two_walleys_id UUID;
    hafnarfjall_ultra_seven_peaks_id UUID;
    hafnarfjall_ultra_summit_id UUID;
    hafnarfjall_ultra_family_run_id UUID;
    hellisheidi_sunnan_vega_id UUID;
    sveifluhals_id UUID;
    ulfarsfellsslaufan_id UUID;
                          
    laugavegur_trail_id UUID := '30000000-0000-0000-0000-000000000001'; -- New, specifically for Laugavegur

BEGIN

-- Insert a test user first (using Supabase's auth.users table)
INSERT INTO auth.users (id, email)
VALUES (seed_created_by, 'svanur@hlaupaleidir.is')
    ON CONFLICT (id) DO NOTHING;

---
--- Insert sample locations
---

-- Top-level parent: Iceland
INSERT INTO locations (id, parent_id, name, slug, description, latitude, longitude, created_by, created_at)
VALUES (
           island_location_id,
           NULL,
           'Ísland',
           'island',
           'The island nation of Iceland.',
           64.9631,
           -19.0208,
           seed_created_by,
           NOW()
       ) ON CONFLICT (id) DO NOTHING;

-- Regions within Iceland
INSERT INTO locations (id, parent_id, name, slug, description, latitude, longitude, created_by, created_at)
VALUES (
           sudurland_location_id,
           island_location_id,
           'Suðurland',
           'sudurland',
           'The South Region of Iceland, known for its diverse landscapes.',
           63.9575,
           -19.8247,
           seed_created_by,
           NOW()
       ) ON CONFLICT (id) DO NOTHING;

INSERT INTO locations (id, parent_id, name, slug, description, latitude, longitude, created_by, created_at)
VALUES (
           hofudborgarsvaedid_location_id,
           island_location_id,
           'Höfuðborgarsvæðið',
           'hofudborgarsvaedid',
           'The Capital Region of Iceland, centered around Reykjavík.',
           64.1283,
           -21.8267,
           seed_created_by,
           NOW()
       ) ON CONFLICT (id) DO NOTHING;

-- Specific locations
INSERT INTO locations (id, parent_id, name, slug, description, latitude, longitude, created_by, created_at)
VALUES (
           thorsmork_location_id,
           sudurland_location_id,
           'Þórsmörk',
           'thorsmork',
           'A beautiful mountain ridge in South Iceland, a popular hiking area.',
           63.6936,
           -19.4674,
           seed_created_by,
           NOW()
       ) ON CONFLICT (id) DO NOTHING;

INSERT INTO locations (id, parent_id, name, slug, description, latitude, longitude, created_by, created_at)
VALUES (
           landmannalaugar_location_id,
           sudurland_location_id,
           'Landmannalaugar',
           'landmannalaugar',
           'A remote area of multicoloured rhyolite mountains, hot springs, and hiking trails.',
           63.9823,
           -19.0601,
           seed_created_by,
           NOW()
       ) ON CONFLICT (id) DO NOTHING;

INSERT INTO locations (id, parent_id, name, slug, description, latitude, longitude, created_by, created_at)
VALUES (
           reykjavik_location_id,
           hofudborgarsvaedid_location_id,
           'Reykjavík',
           'reykjavik',
           'The capital and largest city of Iceland.',
           64.1265,
           -21.8174,
           seed_created_by,
           NOW()
       ) ON CONFLICT (id) DO NOTHING;

---
--- Insert sample trails
---
INSERT INTO public.trails (id, name, slug, description, distance_meters, elevation_gain_meters, difficulty_Level, route_type, terrain_type, surface_type, created_by, created_at)
VALUES
    (gen_random_uuid(), 'Mt Esja Ultra maraþon', 'esja-ultra-marathon-43', 'Maraþon keppnisleiðin í hlíðum Esjunnar.', 43000, 3245, 'unknown', 'unknown','unknown','unknown',seed_created_by, NOW()),
    (gen_random_uuid(), 'Mt Esja Ultra hálfmaraþon', 'esja-ultra-marathon-21', 'Hálfmaraþon keppnisleiðin í hlíðum Esjunnar', 21000, 1433, 'unknown', 'unknown','unknown','unknown', seed_created_by, NOW()),

    (gen_random_uuid(), 'Hengill Ultra 10', 'hengill-ultra-10', 'Hengill Ultra 10km keppnishlaupið í Hveragerði', 52000, 1960, 'unknown', 'unknown','unknown','unknown', seed_created_by, NOW()),
    (gen_random_uuid(), 'Hengill Ultra 25', 'hengill-ultra-25', 'Hengill Ultra 25km keppnishlaupið í Hveragerði', 52000, 1960, 'unknown', 'unknown','unknown','unknown', seed_created_by, NOW()),
    (gen_random_uuid(), 'Hengill Ultra 52', 'hengill-ultra-52', 'Hengill Ultra 52km keppnishlaupið í Hveragerði', 52000, 1960, 'unknown', 'unknown','unknown','unknown', seed_created_by, NOW()),
    (gen_random_uuid(), 'Hengill Ultra 104', 'hengill-ultra-104', 'Hengill Ultra 104km keppnishlaupið í Hveragerði', 52000, 1960, 'unknown', 'unknown','unknown','unknown', seed_created_by, NOW()),

    (gen_random_uuid(), 'Hvítasunnuhlaup Hauka 22km', 'hvitasunnuhlaup-hauka-22', '22km keppnisleiðin í Hvítasunnuhlaupi Hauka', 22000, 451, 'unknown', 'unknown','unknown','unknown', seed_created_by, NOW()),
    (gen_random_uuid(), 'Hvítasunnuhlaup Hauka 17km', 'hvitasunnuhlaup-hauka-17', '17km keppnisleiðin í Hvítasunnuhlaupi Hauka', 17000, 137, 'unknown', 'unknown','unknown','unknown', seed_created_by, NOW()),
    (gen_random_uuid(), 'Hvítasunnuhlaup Hauka 14km', 'hvitasunnuhlaup-hauka-14', '14km keppnisleiðin í Hvítasunnuhlaupi Hauka', 14000, 122, 'unknown', 'unknown','unknown','unknown', seed_created_by, NOW()),
    (gen_random_uuid(), 'Hvítasunnuhlaup Hauka 21km', 'hvitasunnuhlaup-hauka-21', '', 0, 0, 'unknown', 'unknown','unknown','unknown',  seed_created_by, NOW()),

    (gen_random_uuid(), 'Puffin Run', 'puffin-run', 'Puffin Run keppnishlaupið í Vestmannaeygjum', 20000, 295, 'unknown', 'unknown','unknown','unknown', seed_created_by, NOW()),
    (gen_random_uuid(), 'Bakgarður Náttúruhlaupa við Elliðavatn', 'bakgardur-ellidavatn', 'Bakgarður Náttúruhlaupa við Elliðavatn', 6700, 39, 'unknown', 'unknown','unknown','unknown', seed_created_by, NOW()),
    (gen_random_uuid(), '5km hlaup HHFH og 66°N', 'hlaupaseria-66', 'Hlaupasería 66°N og Hlaupahóps FH', 5000, 44, 'unknown', 'unknown','unknown','unknown',seed_created_by, NOW()),
    (gen_random_uuid(), 'Skaftafell Ultra', 'skaftafell-ultra', 'Náttúruhlaup í Skaftafelli', 0, 0, 'unknown', 'unknown','unknown','unknown',  seed_created_by, NOW()),

    (gen_random_uuid(), 'Hafnarfjall Ultra sjö tindar og tveir dalis', 'hafnarfjall-ultra-sjo-tindar-og-tveir-dalir', '7 tindar. Keppnisleið í Hafnarfjall Ultra seríunni', 0, 0, 'unknown', 'unknown','unknown','unknown',  seed_created_by, NOW()),
    (gen_random_uuid(), 'Hafnarfjall Ultra sjö tindar', 'hafnarfjall-ultra-sjo-tindar', '7 tindar og tveir dalir. Keppnisleið í Hafnarfjall Ultra seríunni', 0, 0, 'unknown', 'unknown','unknown','unknown',  seed_created_by, NOW()),
    (gen_random_uuid(), 'Hafnarfjall Ultra 1 tindur', 'hafnarfjall-ultra-summit', '1 tindur. Keppnisleið í Hafnarfjall Ultra seríunni', 0, 0, 'unknown', 'unknown','unknown','unknown',  seed_created_by, NOW()),
    (gen_random_uuid(), 'Hafnarfjall Ultra fjölskylduhlaup', 'hafnarfjall-ultra-fjolskylduhlaup', 'Fjölskylduhlaupið í Hafnarfjall Ultra seríunni', 0, 0, 'unknown', 'unknown','unknown','unknown',  seed_created_by, NOW()),
    
    (gen_random_uuid(), 'Hellisheiði sunnan vega', 'hellisheidi-sunnan-vega', '', 0, 0, 'unknown', 'unknown','unknown','unknown',  seed_created_by, NOW()),
    (gen_random_uuid(), 'Sveifluháls', 'sveifluhals', '', 0, 0, 'unknown', 'unknown','unknown','unknown',  seed_created_by, NOW()),
    (gen_random_uuid(), 'Úlfarsfellsslaufan', 'ulfarsfellsslaufan', '3 Úllar, mismunandi leiðir upp og niður', 0, 0, 'unknown', 'unknown','unknown','unknown',  seed_created_by, NOW()),
    
    (gen_random_uuid(), 'Adidas boost', 'adidas-boost', '', 0, 0, 'unknown', 'unknown','unknown','unknown',  seed_created_by, NOW()),
    (gen_random_uuid(), 'Austur Ultra 18km', 'austur-ultra-18', '', 0, 0, 'unknown', 'unknown','unknown','unknown',  seed_created_by, NOW()),
    (gen_random_uuid(), 'Austur Ultra 50km', 'austur-ultra-50', '', 52000, 1804, 'unknown', 'unknown','unknown','unknown',  seed_created_by, NOW()),
    (gen_random_uuid(), 'Austur Ultra 8km', 'austur-ultra-8', '', 0, 0, 'unknown', 'unknown','unknown','unknown',  seed_created_by, NOW()),

    (gen_random_uuid(), 'Bláskógarskokk 16km', 'blaskogarskokk-16', '', 0, 0, 'unknown', 'unknown','unknown','unknown',  seed_created_by, NOW()),
    (gen_random_uuid(), 'Bláskógarskokk 8km', 'blaskogarskokk-8', '', 0, 0, 'unknown', 'unknown','unknown','unknown',  seed_created_by, NOW()),
    
    (gen_random_uuid(), 'Botnsvatnshlaup 3km', 'botnsvatnshlaup-3', '', 0, 0, 'unknown', 'unknown','unknown','unknown',  seed_created_by, NOW()),
    (gen_random_uuid(), 'Botnvatnshlaup 8km', 'botnvatnshlaup-8', '', 0, 0, 'unknown', 'unknown','unknown','unknown',  seed_created_by, NOW()),

    (gen_random_uuid(), 'Eldslóðin 10km', 'eldslodin-10', '', 0, 0, 'unknown', 'unknown','unknown','unknown',  seed_created_by, NOW()),
    (gen_random_uuid(), 'Eldslóðin 29km', 'eldslodin-29', '', 0, 0, 'unknown', 'unknown','unknown','unknown',  seed_created_by, NOW()),
    (gen_random_uuid(), 'Eldslóðin 5km', 'eldslodin-5', '', 0, 0, 'unknown', 'unknown','unknown','unknown',  seed_created_by, NOW()),
    
    (gen_random_uuid(), 'Fimmvörðuhálshlaupið', 'fimmvorduhalshlaupid', '', 28000, 1048, 'unknown', 'unknown','unknown','unknown',  seed_created_by, NOW()),
    
    (gen_random_uuid(), 'Pósthlaupið 26km', 'posthlaupid-26', '', 0, 0, 'unknown', 'unknown','unknown','unknown',  seed_created_by, NOW()),
    (gen_random_uuid(), 'Pósthlaupið 50km', 'posthlaupid-50', '', 0, 0, 'unknown', 'unknown','unknown','unknown',  seed_created_by, NOW()),
    (gen_random_uuid(), 'Pósthlaupið 12km', 'posthlaupid-12', '', 0, 0, 'unknown', 'unknown','unknown','unknown',  seed_created_by, NOW()),
    (gen_random_uuid(), 'Pósthlaupið 7km', 'posthlaupid-7', '', 0, 0, 'unknown', 'unknown','unknown','unknown',  seed_created_by, NOW()),
    
    (gen_random_uuid(), 'Súlur Vertical - Fálkinn', 'sulur-vertical-falkinn', '', 18000, 543, 'unknown', 'unknown','unknown','unknown',  seed_created_by, NOW()),
    (gen_random_uuid(), 'Súlur Vertical - Gyðjan', 'sulur-vertical-gydjan', '', 100000, 3920, 'unknown', 'unknown','unknown','unknown',  seed_created_by, NOW()),    
    (gen_random_uuid(), 'Súlur Vertical - Súlur', 'sulur-vertical-sulur', '', 29000, 1334, 'unknown', 'unknown','unknown','unknown',  seed_created_by, NOW()),
    (gen_random_uuid(), 'Súlur Vertical - Tröllið', 'sulur-vertical-trollid', '', 43000, 1906, 'unknown', 'unknown','unknown','unknown',  seed_created_by, NOW()),
    
    (gen_random_uuid(), 'Vatnsmýrarhlaupið', 'vatnsmyrarhlaupid', '', 0, 0, 'unknown', 'unknown','unknown','unknown',  seed_created_by, NOW()),
    (gen_random_uuid(), 'Vatnsnes Trail run - 10km', 'vatnsnes-trail-run-10', '', 0, 0, 'unknown', 'unknown','unknown','unknown',  seed_created_by, NOW()),
    (gen_random_uuid(), 'Vatnsnes Trail run- 20km', 'vatnsnes-trail-run-20', '', 0, 0, 'unknown', 'unknown','unknown','unknown',  seed_created_by, NOW()),
    
    (gen_random_uuid(), 'Akranes Ultra 10', 'akranes-ultra-10', '', 10000, 30, 'unknown', 'unknown','unknown','unknown',  seed_created_by, NOW()),
    (gen_random_uuid(), 'Akranes Ultra 20', 'akranes-ultra-20', '', 20000, 560, 'unknown', 'unknown','unknown','unknown',  seed_created_by, NOW()),
    (gen_random_uuid(), 'Akranes Ultra 27', 'akranes-ultra-27', '', 27000, 1150, 'unknown', 'unknown','unknown','unknown',  seed_created_by, NOW()),

    (gen_random_uuid(), 'Kerlingafjöll Ultra 12', 'kerlingafjoll-ultra-12', '', 12000, 560, 'unknown', 'unknown','unknown','unknown',  seed_created_by, NOW()),
    (gen_random_uuid(), 'Kerlingafjöll Ultra 22', 'kerlingafjoll-ultra-22', '', 22000, 1206, 'unknown', 'unknown','unknown','unknown',  seed_created_by, NOW()),
    (gen_random_uuid(), 'Kerlingafjöll Ultra 63', 'kerlingafjoll-ultra-63', '', 63000, 2377, 'unknown', 'unknown','unknown','unknown',  seed_created_by, NOW()),
    
    (gen_random_uuid(), 'Mýrdalshlaupið 10km', 'myrdalshlaupid-10', '', 10000, 297, 'unknown', 'unknown','unknown','unknown',  seed_created_by, NOW()),
    (gen_random_uuid(), 'Mýrdalshlaupið 22km', 'myrdalshlaupid-22', '', 22000, 1094, 'unknown', 'unknown','unknown','unknown',  seed_created_by, NOW()),
    (gen_random_uuid(), 'Volcano Trail Run', 'volcano-trail-run', '', 12200, 659, 'unknown', 'unknown','unknown','unknown',  seed_created_by, NOW()),

    (gen_random_uuid(), 'Tindahlaupið, 1 tindur', 'tindahlaupid-1', '', 12000, 203, 'unknown', 'unknown','unknown','unknown',  seed_created_by, NOW()),
    (gen_random_uuid(), 'Tindahlaupið, 3 tindar', 'tindahlaupid-3', '', 18000, 720, 'unknown', 'unknown','unknown','unknown',  seed_created_by, NOW()),
    (gen_random_uuid(), 'Tindahlaupið, 5 tindar', 'tindahlaupid-5', '', 34000, 1196, 'unknown', 'unknown','unknown','unknown',  seed_created_by, NOW()),
    (gen_random_uuid(), 'Tindahlaupið, 7 tindar', 'tindahlaupid-7', '', 37000, 1569, 'unknown', 'unknown','unknown','unknown',  seed_created_by, NOW()),
    
    ( laugavegur_trail_id, 'Laugavegur Ultra', 'laugavegur-ultra', 'Laugavegshlaupið er 55 km utanvegahlaup en Laugavegurinn tengir saman Landmannalaugar og Þórsmörk á sunnanverðu hálendi Íslands, tvær sannkallaðar náttúruperlur. Göngugarpar eru venjulega 4 daga á leið sinni um Laugaveginn en hröðustu hlaupararnir fara leiðina á 4-5 klukkustundum.', 53000, 1293, 'unknown', 'unknown','unknown','unknown', seed_created_by, NOW()
) ON CONFLICT (slug) DO NOTHING;

---
--- Insert sample races
---

-- Laugavegur Ultra Marathon
INSERT INTO public.races (id, location_id, name, slug, description, web_url, race_status, recurring_month, recurring_week, recurring_weekday, created_by, created_at)
VALUES (
           laugavegur_race_id,
           landmannalaugar_location_id, -- Primary location for race (used as fallback in older schema)
           'Laugavegur Ultra Marathon',
           'laugavegur-ultra',
           'Iconic 55km mountain trail race from Landmannalaugar to Þórsmörk.',
           'https://www.laugavegshlaup.is/',
           'active',
           7, -- July
           3, -- Third week
           6, -- Saturday (1=Mon, 7=Sun)
           seed_created_by,
           NOW()
       ) ON CONFLICT (id) DO NOTHING;

-- Hengill Ultra
INSERT INTO public.races (id, location_id, name, slug, description, web_url, race_status, recurring_month, recurring_week, recurring_weekday, created_by, created_at)
VALUES (
           hengill_ultra_race_id,
           sudurland_location_id, -- General location for the race
           'Hengill Ultra',
           'hengill-ultra',
           'Trail running event in the geothermal Hengill area with multiple distance_meterss.',
           'https://www.vikingamot.is/hengill-ultra/', 
           'active',
           6, -- June
           1, -- First week
           6, -- Saturday
           seed_created_by,
           NOW()
       ) ON CONFLICT (id) DO NOTHING;

-- Reykjavík Marathon
INSERT INTO public.races (id, location_id, name, slug, description, race_status, recurring_month, recurring_week, recurring_weekday, created_by, created_at)
VALUES (
           reykjavik_marathon_race_id,
           reykjavik_location_id, -- Primary location is Reykjavik city
           'Reykjavík Marathon',
           'reykjavik-marathon',
           'Annual road running event in the capital city, offering various distance_meterss.',
           'active',
           8, -- August
           3, -- Third week
           6, -- Saturday
           seed_created_by,
           NOW()
       ) ON CONFLICT (id) DO NOTHING;

-- NEW: Esja Ultra Race (conceptual race for Esja trails)
INSERT INTO public.races (id, location_id, name, slug, description, race_status, recurring_month, recurring_week, recurring_weekday, created_by, created_at)
VALUES (esja_ultra_race_id, reykjavik_location_id, 'Mt Esja Ultra', 'mt-esja-ultra', 'Mountain ultra marathon on Mt. Esja, offering multiple distance_meterss.', 'active', 7, 4, 6, seed_created_by, NOW())
    ON CONFLICT (id) DO NOTHING;

-- NEW: Hvítasunnuhlaup Hauka Race (conceptual race)
INSERT INTO public.races (id, location_id, name, slug, description, race_status, recurring_month, recurring_week, recurring_weekday, created_by, created_at)
VALUES (hvitasunnuhlaup_hauka_race_id, reykjavik_location_id, 'Hvítasunnuhlaup Hauka', 'hvitasunnuhlaup-hauka', 'Annual race by Haukar athletic club, held near Hafnarfjörður.', 'active', 6, 2, 7, seed_created_by, NOW())
    ON CONFLICT (id) DO NOTHING;

---
--- Fetch trail IDs by slug AFTER trails have been inserted
---
SELECT id INTO esja_ultra_marathon_trail_id FROM public.trails WHERE slug = 'esja-ultra-marathon-43';
SELECT id INTO esja_ultra_half_marathon_trail_id FROM public.trails WHERE slug = 'esja-ultra-marathon-21';
SELECT id INTO hengill_ultra_52_trail_id FROM public.trails WHERE slug = 'hengill-ultra-52';
SELECT id INTO hvitasunnuhlaup_hauka_22_trail_id FROM public.trails WHERE slug = 'hvitasunnuhlaup-hauka-22';
SELECT id INTO hvitasunnuhlaup_hauka_17_trail_id FROM public.trails WHERE slug = 'hvitasunnuhlaup-hauka-17';
SELECT id INTO hvitasunnuhlaup_hauka_14_trail_id FROM public.trails WHERE slug = 'hvitasunnuhlaup-hauka-14';
SELECT id INTO puffin_run_trail_id FROM public.trails WHERE slug = 'puffin-run';

SELECT id INTO hafnarfjall_ultra_seven_peaks_and_two_walleys_id FROM public.trails WHERE slug = 'hafnarfjall-ultra-sjo-tindar-og-tveir-dalir';
SELECT id INTO hafnarfjall_ultra_seven_peaks_id FROM public.trails WHERE slug = 'hafnarfjall-ultra-sjo-tindar';
SELECT id INTO hafnarfjall_ultra_summit_id FROM public.trails WHERE slug = 'hafnarfjall-ultra-summit';
SELECT id INTO hafnarfjall_ultra_family_run_id FROM public.trails WHERE slug = 'hafnarfjall-ultra-family-run';
SELECT id INTO hellisheidi_sunnan_vega_id FROM public.trails WHERE slug = 'hellisheidi-sunnan-vega';
SELECT id INTO sveifluhals_id FROM public.trails WHERE slug = 'sveifluhals';
SELECT id INTO ulfarsfellsslaufan_id FROM public.trails WHERE slug = 'ulfarsfellsslaufan';

---
--- Insert sample race_trails connections
---

-- Laugavegur Ultra Marathon
INSERT INTO public.race_trails (race_id, trail_id, race_status, comment, display_order, created_at, created_by)
VALUES (laugavegur_race_id, laugavegur_trail_id, 'active', NULL, 1, NOW(), seed_created_by)
    ON CONFLICT (race_id, trail_id) DO NOTHING;

-- Hengill Ultra Race
INSERT INTO public.race_trails (race_id, trail_id, race_status, comment, display_order, created_at, created_by)
VALUES (hengill_ultra_race_id, hengill_ultra_52_trail_id, 'active', 'Main 52km course.', 1, NOW(), seed_created_by)
    ON CONFLICT (race_id, trail_id) DO NOTHING;

-- Mt Esja Ultra
INSERT INTO public.race_trails (race_id, trail_id, race_status, comment, display_order, created_at, created_by)
VALUES (esja_ultra_race_id, esja_ultra_marathon_trail_id, 'active', 'The full marathon course for Esja Ultra.', 1, NOW(), seed_created_by)
    ON CONFLICT (race_id, trail_id) DO NOTHING;

INSERT INTO public.race_trails (race_id, trail_id, race_status, comment, display_order, created_at, created_by)
VALUES (esja_ultra_race_id, esja_ultra_half_marathon_trail_id, 'active', 'The half marathon course for Esja Ultra.', 2, NOW(), seed_created_by)
    ON CONFLICT (race_id, trail_id) DO NOTHING;

-- Hvítasunnuhlaup Hauka
INSERT INTO public.race_trails (race_id, trail_id, race_status, comment, display_order, created_at, created_by)
VALUES (hvitasunnuhlaup_hauka_race_id, hvitasunnuhlaup_hauka_22_trail_id, 'active', 'Longest distance_meters for the event.', 1, NOW(), seed_created_by)
    ON CONFLICT (race_id, trail_id) DO NOTHING;

INSERT INTO public.race_trails (race_id, trail_id, race_status, comment, display_order, created_at, created_by)
VALUES (hvitasunnuhlaup_hauka_race_id, hvitasunnuhlaup_hauka_17_trail_id, 'active', NULL, 2, NOW(), seed_created_by)
    ON CONFLICT (race_id, trail_id) DO NOTHING;

INSERT INTO public.race_trails (race_id, trail_id, race_status, comment, display_order, created_at, created_by)
VALUES (hvitasunnuhlaup_hauka_race_id, hvitasunnuhlaup_hauka_14_trail_id, 'active', 'Shortest distance_meters, suitable for beginners.', 3, NOW(), seed_created_by)
    ON CONFLICT (race_id, trail_id) DO NOTHING;

---
--- Insert sample race_locations connections
---

-- Laugavegur Ultra Marathon (laugavegur_race_id)
-- Start: Landmannalaugar
-- End: Þórsmörk
INSERT INTO public.race_locations (race_id, location_id, location_type, display_order, comment, created_at, created_by)
VALUES (laugavegur_race_id, landmannalaugar_location_id, 'start', 1, 'Official race start.', NOW(), seed_created_by)
    ON CONFLICT (race_id, location_id) DO NOTHING;

INSERT INTO public.race_locations (race_id, location_id, location_type, display_order, comment, created_at, created_by)
VALUES (laugavegur_race_id, thorsmork_location_id, 'end', 2, 'Official race finish line.', NOW(), seed_created_by)
    ON CONFLICT (race_id, location_id) DO NOTHING;
-- (You could add aid stations or checkpoints here if you had those locations)

-- Hengill Ultra (hengill_ultra_race_id)
-- Main location around Hveragerði, which is within Sudurland. Let's use Sudurland as the general area.
-- If you had a more specific "Hveragerði" location, you'd use that.
INSERT INTO public.race_locations (race_id, location_id, location_type, display_order, comment, created_at, created_by)
VALUES (hengill_ultra_race_id, sudurland_location_id, 'unknown', 1, 'General area for the race, specific start/end varies by distance_meters.', NOW(), seed_created_by)
    ON CONFLICT (race_id, location_id) DO NOTHING;


-- Reykjavík Marathon (reykjavik_marathon_race_id)
-- Start/End: Reykjavík
INSERT INTO public.race_locations (race_id, location_id, location_type, display_order, comment, created_at, created_by)
VALUES (reykjavik_marathon_race_id, reykjavik_location_id, 'start', 1, 'Race starts and finishes in downtown Reykjavík.', NOW(), seed_created_by)
    ON CONFLICT (race_id, location_id) DO NOTHING;

INSERT INTO public.race_locations (race_id, location_id, location_type, display_order, comment, created_at, created_by)
VALUES (reykjavik_marathon_race_id, reykjavik_location_id, 'end', 2, 'Race finishes in downtown Reykjavík.', NOW(), seed_created_by)
    ON CONFLICT (race_id, location_id) DO NOTHING;

-- Mt Esja Ultra (esja_ultra_race_id)
-- Location: Reykjavík (assuming Esja area is conceptually linked to Reykjavík for location)
INSERT INTO public.race_locations (race_id, location_id, location_type, display_order, comment, created_at, created_by)
VALUES (esja_ultra_race_id, reykjavik_location_id, 'unknown', 1, 'Race located on Mt. Esja, near Reykjavík.', NOW(), seed_created_by)
    ON CONFLICT (race_id, location_id) DO NOTHING;

-- Hvítasunnuhlaup Hauka (hvitasunnuhlaup_hauka_race_id)
-- Location: Reykjavík (assuming it's in the greater capital area)
INSERT INTO public.race_locations (race_id, location_id, location_type, display_order, comment, created_at, created_by)
VALUES (hvitasunnuhlaup_hauka_race_id, reykjavik_location_id, 'unknown', 1, 'Race takes place in the greater Reykjavík area.', NOW(), seed_created_by)
    ON CONFLICT (race_id, location_id) DO NOTHING;

---
--- Insert sample trail_locations connections
---

-- Laugavegur Trail (laugavegur_trail_id)
-- Start: Landmannalaugar
-- End: Þórsmörk
INSERT INTO public.trail_locations (trail_id, location_id, location_type, display_order, comment, created_at, created_by)
VALUES (laugavegur_trail_id, landmannalaugar_location_id, 'start', 1, 'Northern trailhead of Laugavegur.', NOW(), seed_created_by)
    ON CONFLICT (trail_id, location_id) DO NOTHING;

INSERT INTO public.trail_locations (trail_id, location_id, location_type, display_order, comment, created_at, created_by)
VALUES (laugavegur_trail_id, thorsmork_location_id, 'end', 2, 'Southern trailhead of Laugavegur.', NOW(), seed_created_by)
    ON CONFLICT (trail_id, location_id) DO NOTHING;

-- Hengill Ultra 52 (hengill_ultra_52_trail_id)
-- Location: Suðurland (Hveragerði area)
INSERT INTO public.trail_locations (trail_id, location_id, location_type, display_order, comment, created_at, created_by)
VALUES (hengill_ultra_52_trail_id, sudurland_location_id, 'unknown', 1, 'Trail primarily located within the Hengill geothermal area.', NOW(), seed_created_by)
    ON CONFLICT (trail_id, location_id) DO NOTHING;

-- Mt Esja Ultra marathon & half marathon trails (esja_ultra_marathon_trail_id, esja_ultra_half_marathon_trail_id)
-- Location: Reykjavík
INSERT INTO public.trail_locations (trail_id, location_id, location_type, display_order, comment, created_at, created_by)
VALUES (esja_ultra_marathon_trail_id, reykjavik_location_id, 'unknown', 1, 'Trail on Mt. Esja, easily accessible from Reykjavík.', NOW(), seed_created_by)
    ON CONFLICT (trail_id, location_id) DO NOTHING;

INSERT INTO public.trail_locations (trail_id, location_id, location_type, display_order, comment, created_at, created_by)
VALUES (esja_ultra_half_marathon_trail_id, reykjavik_location_id, 'unknown', 1, 'Trail on Mt. Esja, easily accessible from Reykjavík.', NOW(), seed_created_by)
    ON CONFLICT (trail_id, location_id) DO NOTHING;

-- Hvítasunnuhlaup Hauka trails (22km, 17km, 14km)
-- Location: Reykjavík (or more specifically, Hafnarfjörður if you had that location)
INSERT INTO public.trail_locations (trail_id, location_id, location_type, display_order, comment, created_at, created_by)
VALUES (hvitasunnuhlaup_hauka_22_trail_id, reykjavik_location_id, 'unknown', 1, 'Trail in the capital region.', NOW(), seed_created_by)
    ON CONFLICT (trail_id, location_id) DO NOTHING;

INSERT INTO public.trail_locations (trail_id, location_id, location_type, display_order, comment, created_at, created_by)
VALUES (hvitasunnuhlaup_hauka_17_trail_id, reykjavik_location_id, 'unknown', 1, 'Trail in the capital region.', NOW(), seed_created_by)
    ON CONFLICT (trail_id, location_id) DO NOTHING;

INSERT INTO public.trail_locations (trail_id, location_id, location_type, display_order, comment, created_at, created_by)
VALUES (hvitasunnuhlaup_hauka_14_trail_id, reykjavik_location_id, 'unknown', 1, 'Trail in the capital region.', NOW(), seed_created_by)
    ON CONFLICT (trail_id, location_id) DO NOTHING;
 
-- Puffin Run (puffin_run_trail_id)
-- Location: Vestmannaeyjar (if you add a location for it, otherwise default to Iceland or nearest region)
-- For now, let's connect it to Island if you don't have a specific Vestmannaeyjar location.
INSERT INTO public.trail_locations (trail_id, location_id, location_type, display_order, comment, created_at, created_by)
VALUES (puffin_run_trail_id, island_location_id, 'unknown', 1, 'Trail on Vestmannaeyjar (Westman Islands).', NOW(), seed_created_by)
    ON CONFLICT (trail_id, location_id) DO NOTHING;

END $$;