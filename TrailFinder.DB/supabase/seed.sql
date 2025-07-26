DO $$
DECLARE
    -- Declare UUID variables for locations to establish parent-child relationships
    island_id UUID := '10000000-0000-0000-0000-000000000001';
    sudurland_id UUID := '10000000-0000-0000-0000-000000000002';
    hofudborgarsvaedid_id UUID := '10000000-0000-0000-0000-000000000003';
    thorsmork_id UUID := '10000000-0000-0000-0000-000000000004';
    landmannalaugar_id UUID := '10000000-0000-0000-0000-000000000005';
    reykjavik_id UUID := '10000000-0000-0000-0000-000000000006';
    
    -- Placeholder for a created_by (replace with an actual user from your auth.users table if required)
    seed_created_by UUID := '00000000-0000-0000-0000-000000000001';

    -- Declare UUID variables for new race IDs
    laugavegur_id UUID := '20000000-0000-0000-0000-000000000001';
    hengill_ultra_id UUID := '20000000-0000-0000-0000-000000000002';
    reykjavik_marathon_id UUID := '20000000-0000-0000-0000-000000000003';
    ice_ultra_id UUID := '20000000-0000-0000-0000-000000000004';
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
           island_id,
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
           sudurland_id,
           island_id,
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
           hofudborgarsvaedid_id,
           island_id,
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
           thorsmork_id,
           sudurland_id,
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
           landmannalaugar_id,
           sudurland_id,
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
           reykjavik_id,
           hofudborgarsvaedid_id,
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
    (gen_random_uuid(), 'Mt Esja Ultra maraþon', 'esja-ultra-marathon', 'Maraþon keppnisleiðin í hlíðum Esjunnar.', 43, 3245, 'unknown', 'unknown','unknown','unknown',seed_created_by, NOW()),
    (gen_random_uuid(), 'Mt Esja Ultra hálfmaraþon', 'esja-ultra-half-marathon', 'Hálfmaraþon keppnisleiðin í hlíðum Esjunnar', 0, 0, 'unknown', 'unknown','unknown','unknown', seed_created_by, NOW()),
    (gen_random_uuid(), 'Hengill Ultra 52', 'hengill-ultra-52', 'Hengill Ultra keppnishlaupið í Hveragerði', 0, 0, 'unknown', 'unknown','unknown','unknown', seed_created_by, NOW()),
    (gen_random_uuid(), 'Hvítasunnuhlaup Hauka 22km', 'hvitasunnuhlaup-hauka-22', '22km keppnisleiðin í Hvítasunnuhlaupi Hauka', 0, 0, 'unknown', 'unknown','unknown','unknown', seed_created_by, NOW()),
    (gen_random_uuid(), 'Hvítasunnuhlaup Hauka 17km', 'hvitasunnuhlaup-hauka-17', '17km keppnisleiðin í Hvítasunnuhlaupi Hauka', 0, 0, 'unknown', 'unknown','unknown','unknown', seed_created_by, NOW()),
    (gen_random_uuid(), 'Hvítasunnuhlaup Hauka 14km', 'hvitasunnuhlaup-hauka-14', '14km keppnisleiðin í Hvítasunnuhlaupi Hauka', 0, 0, 'unknown', 'unknown','unknown','unknown', seed_created_by, NOW()),
    (gen_random_uuid(), 'Puffin Run', 'puffin-run', 'Puffin Run keppnishlaupið í Vestmannaeygjum', 0, 0, 'unknown', 'unknown','unknown','unknown', seed_created_by, NOW()),
    (gen_random_uuid(), 'Bakgarður Náttúruhlaupa við Elliðavatn', 'bakgardur-ellidavatn', 'Bakgarður Náttúruhlaupa við Elliðavatn', 0, 0, 'unknown', 'unknown','unknown','unknown', seed_created_by, NOW()),
    (gen_random_uuid(), '5km hlaup HHFH og 66°N', 'hlaupaseria-66', 'Hlaupasería 66°N og Hlaupahóps FH', 5, 44, 'unknown', 'unknown','unknown','unknown',seed_created_by, NOW()),
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
    (gen_random_uuid(), 'Austur Ultra 50km', 'austur-ultra-50', '', 0, 0, 'unknown', 'unknown','unknown','unknown',  seed_created_by, NOW()),
    (gen_random_uuid(), 'Austur Ultra 8km', 'austur-ultra-8', '', 0, 0, 'unknown', 'unknown','unknown','unknown',  seed_created_by, NOW()),
    (gen_random_uuid(), 'Bláskógarskokk 16km', 'blaskogarskokk-16', '', 0, 0, 'unknown', 'unknown','unknown','unknown',  seed_created_by, NOW()),
    (gen_random_uuid(), 'Bláskógarskokk 8km', 'blaskogarskokk-8', '', 0, 0, 'unknown', 'unknown','unknown','unknown',  seed_created_by, NOW()),
    
    (gen_random_uuid(), 'Botnsvatnshlaup 3km', 'botnsvatnshlaup-3', '', 0, 0, 'unknown', 'unknown','unknown','unknown',  seed_created_by, NOW()),
    (gen_random_uuid(), 'Botnvatnshlaup 8km', 'botnvatnshlaup-8', '', 0, 0, 'unknown', 'unknown','unknown','unknown',  seed_created_by, NOW()),
    (gen_random_uuid(), 'Eldslóðin 10km', 'eldslodin-10', '', 0, 0, 'unknown', 'unknown','unknown','unknown',  seed_created_by, NOW()),
    (gen_random_uuid(), 'Eldslóðin 29km', 'eldslodin-29', '', 0, 0, 'unknown', 'unknown','unknown','unknown',  seed_created_by, NOW()),
    (gen_random_uuid(), 'Eldslóðin 5km', 'eldslodin-5', '', 0, 0, 'unknown', 'unknown','unknown','unknown',  seed_created_by, NOW()),
    
    (gen_random_uuid(), 'Fimmvörðuhálshlaupið', 'fimmvorduhalshlaupid', '', 0, 0, 'unknown', 'unknown','unknown','unknown',  seed_created_by, NOW()),
    (gen_random_uuid(), 'Hvítasunnuhlaup Hauka 21km', 'hvitasunnuhlaup-hauka-21', '', 0, 0, 'unknown', 'unknown','unknown','unknown',  seed_created_by, NOW()),
    (gen_random_uuid(), 'Pósthlaupið 7km', 'posthlaupid-7', '', 0, 0, 'unknown', 'unknown','unknown','unknown',  seed_created_by, NOW()),
    (gen_random_uuid(), 'Austur Ultra 50km', 'austur-ultra-50', '', 0, 0, 'unknown', 'unknown','unknown','unknown',  seed_created_by, NOW()),
    
    (gen_random_uuid(), 'Pósthlaupið 26km', 'posthlaupid-26', '', 0, 0, 'unknown', 'unknown','unknown','unknown',  seed_created_by, NOW()),
    (gen_random_uuid(), 'Pósthlaupið 50km', 'posthlaupid-50', '', 0, 0, 'unknown', 'unknown','unknown','unknown',  seed_created_by, NOW()),
    (gen_random_uuid(), 'Pósthlaupið 12km', 'posthlaupid-12', '', 0, 0, 'unknown', 'unknown','unknown','unknown',  seed_created_by, NOW()),
    (gen_random_uuid(), 'Súlur Vertical - Fálkinn', 'sulur-vertical-falkinn', '', 0, 0, 'unknown', 'unknown','unknown','unknown',  seed_created_by, NOW()),
    (gen_random_uuid(), 'Súlur Vertical - Gyðjan', 'sulur-vertical-gydjan', '', 0, 0, 'unknown', 'unknown','unknown','unknown',  seed_created_by, NOW()),
    
    (gen_random_uuid(), 'Súlur Vertical - Súlur', 'sulur-vertical-sulur', '', 0, 0, 'unknown', 'unknown','unknown','unknown',  seed_created_by, NOW()),
    (gen_random_uuid(), 'Súlur Vertical - Tröllið', 'sulur-vertical-trollid', '', 0, 0, 'unknown', 'unknown','unknown','unknown',  seed_created_by, NOW()),
    (gen_random_uuid(), 'Vatnsmýrarhlaupið', 'vatnsmyrarhlaupid', '', 0, 0, 'unknown', 'unknown','unknown','unknown',  seed_created_by, NOW()),
    (gen_random_uuid(), 'Vatnsnes Trail run - 10km', 'vatnsnes-trail-run-10', '', 0, 0, 'unknown', 'unknown','unknown','unknown',  seed_created_by, NOW()),
    (gen_random_uuid(), 'Vatnsnes Trail run- 20km', 'vatnsnes-trail-run-20', '', 0, 0, 'unknown', 'unknown','unknown','unknown',  seed_created_by, NOW()),
    
    ( laugavegur_trail_id, 'Laugavegur Trail', 'laugavegur-ultra', 'Laugavegshlaupið er 55 km utanvegahlaup en Laugavegurinn tengir saman Landmannalaugar og Þórsmörk á sunnanverðu hálendi Íslands, tvær sannkallaðar náttúruperlur. Göngugarpar eru venjulega 4 daga á leið sinni um Laugaveginn en hröðustu hlaupararnir fara leiðina á 4-5 klukkustundum.', 53, 1500, 'unknown', 'unknown','unknown','unknown', seed_created_by, NOW()
) ON CONFLICT (slug) DO NOTHING;

---
--- Insert sample races
---

-- Laugavegur Ultra Marathon
INSERT INTO public.races (id, location_id, name, slug, description, web_url, race_status, recurring_month, recurring_week, recurring_weekday, created_by, created_at)
VALUES (
           laugavegur_id,
           landmannalaugar_id, -- Primary location for race (used as fallback in older schema)
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
           hengill_ultra_id,
           sudurland_id, -- General location for the race
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
           reykjavik_marathon_id,
           reykjavik_id, -- Primary location is Reykjavik city
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

-- Ice Ultra (example of a less frequent/maybe deprecated race)
INSERT INTO public.races (id, location_id, name, slug, description, race_status, recurring_month, recurring_week, recurring_weekday, created_by, created_at)
VALUES (
           ice_ultra_id,
           island_id, -- General location
           'Ice Ultra',
           'ice-ultra',
           'A challenging multi-stage winter ultra-marathon across the Icelandic wilderness.',
           'deprecated', -- Example of a deprecated/historical race
           3, -- March
           1, -- First week
           1, -- Monday
           seed_created_by,
           NOW()
       ) ON CONFLICT (id) DO NOTHING;

-- NEW: Esja Ultra Race (conceptual race for Esja trails)
INSERT INTO public.races (id, location_id, name, slug, description, race_status, recurring_month, recurring_week, recurring_weekday, created_by, created_at)
VALUES (esja_ultra_race_id, reykjavik_id, 'Mt Esja Ultra', 'mt-esja-ultra', 'Mountain ultra marathon on Mt. Esja, offering multiple distance_meterss.', 'active', 7, 4, 6, seed_created_by, NOW())
    ON CONFLICT (id) DO NOTHING;

-- NEW: Hvítasunnuhlaup Hauka Race (conceptual race)
INSERT INTO public.races (id, location_id, name, slug, description, race_status, recurring_month, recurring_week, recurring_weekday, created_by, created_at)
VALUES (hvitasunnuhlaup_hauka_race_id, reykjavik_id, 'Hvítasunnuhlaup Hauka', 'hvitasunnuhlaupid', 'Annual race by Haukar athletic club, held near Hafnarfjörður.', 'active', 6, 2, 7, seed_created_by, NOW())
    ON CONFLICT (id) DO NOTHING;

---
--- Fetch trail IDs by slug AFTER trails have been inserted
---
SELECT id INTO esja_ultra_marathon_trail_id FROM public.trails WHERE slug = 'esja-ultra-marathon';
SELECT id INTO esja_ultra_half_marathon_trail_id FROM public.trails WHERE slug = 'esja-ultra-half-marathon';
SELECT id INTO hengill_ultra_52_trail_id FROM public.trails WHERE slug = 'hengill-ultra-52';
SELECT id INTO hvitasunnuhlaup_hauka_22_trail_id FROM public.trails WHERE slug = 'hvitasunnuhlaup-hauka-22';
SELECT id INTO hvitasunnuhlaup_hauka_17_trail_id FROM public.trails WHERE slug = 'hvitasunnuhlaup-hauka-17';
SELECT id INTO hvitasunnuhlaup_hauka_14_trail_id FROM public.trails WHERE slug = 'hvitasunnuhlaup-hauka-14';
SELECT id INTO puffin_run_trail_id FROM public.trails WHERE slug = 'puffin-run';
SELECT id INTO bakgardur_ellidavatn_trail_id FROM public.trails WHERE slug = 'bakgardur-ellidavatn';
SELECT id INTO skaftafell_ultra_trail_id FROM public.trails WHERE slug = 'skaftafell-ultra';

SELECT id INTO hafnarfjall_ultra_seven_peaks_and_two_walleys_id FROM public.trails WHERE slug = 'hafnarfjall-ultra-seven-peaks-and-two-walleys';
SELECT id INTO hafnarfjall_ultra_seven_peaks_id FROM public.trails WHERE slug = 'hafnarfjall-ultra-seven-peaks';
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
VALUES (laugavegur_id, laugavegur_trail_id, 'active', NULL, 1, NOW(), seed_created_by)
    ON CONFLICT (race_id, trail_id) DO NOTHING;

-- Hengill Ultra Race
INSERT INTO public.race_trails (race_id, trail_id, race_status, comment, display_order, created_at, created_by)
VALUES (hengill_ultra_id, hengill_ultra_52_trail_id, 'active', 'Main 52km course.', 1, NOW(), seed_created_by)
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

-- Reykjavík Marathon
INSERT INTO public.race_trails (race_id, trail_id, race_status, comment, display_order, created_at, created_by)
VALUES (reykjavik_marathon_id, bakgardur_ellidavatn_trail_id, 'unknown', 'Used for a popular shorter distance_meters, official course may vary slightly.', NULL, NOW(), seed_created_by)
    ON CONFLICT (race_id, trail_id) DO NOTHING;

-- Ice Ultra
INSERT INTO public.race_trails (race_id, trail_id, race_status, comment, display_order, created_at, created_by)
VALUES (ice_ultra_id, skaftafell_ultra_trail_id, 'deprecated', 'This specific trail was part of the 2020 edition, but the event itself is deprecated.', 1, NOW(), seed_created_by)
    ON CONFLICT (race_id, trail_id) DO NOTHING;

---
--- Insert sample race_locations connections
---

-- Laugavegur Ultra Marathon (laugavegur_id)
-- Start: Landmannalaugar
-- End: Þórsmörk
INSERT INTO public.race_locations (race_id, location_id, location_type, display_order, comment, created_at, created_by)
VALUES (laugavegur_id, landmannalaugar_id, 'start', 1, 'Official race start.', NOW(), seed_created_by)
    ON CONFLICT (race_id, location_id) DO NOTHING;

INSERT INTO public.race_locations (race_id, location_id, location_type, display_order, comment, created_at, created_by)
VALUES (laugavegur_id, thorsmork_id, 'end', 2, 'Official race finish line.', NOW(), seed_created_by)
    ON CONFLICT (race_id, location_id) DO NOTHING;
-- (You could add aid stations or checkpoints here if you had those locations)

-- Hengill Ultra (hengill_ultra_id)
-- Main location around Hveragerði, which is within Sudurland. Let's use Sudurland as the general area.
-- If you had a more specific "Hveragerði" location, you'd use that.
INSERT INTO public.race_locations (race_id, location_id, location_type, display_order, comment, created_at, created_by)
VALUES (hengill_ultra_id, sudurland_id, 'unknown', 1, 'General area for the race, specific start/end varies by distance_meters.', NOW(), seed_created_by)
    ON CONFLICT (race_id, location_id) DO NOTHING;


-- Reykjavík Marathon (reykjavik_marathon_id)
-- Start/End: Reykjavík
INSERT INTO public.race_locations (race_id, location_id, location_type, display_order, comment, created_at, created_by)
VALUES (reykjavik_marathon_id, reykjavik_id, 'start', 1, 'Race starts and finishes in downtown Reykjavík.', NOW(), seed_created_by)
    ON CONFLICT (race_id, location_id) DO NOTHING;

INSERT INTO public.race_locations (race_id, location_id, location_type, display_order, comment, created_at, created_by)
VALUES (reykjavik_marathon_id, reykjavik_id, 'end', 2, 'Race finishes in downtown Reykjavík.', NOW(), seed_created_by)
    ON CONFLICT (race_id, location_id) DO NOTHING;

-- Mt Esja Ultra (esja_ultra_race_id)
-- Location: Reykjavík (assuming Esja area is conceptually linked to Reykjavík for location)
INSERT INTO public.race_locations (race_id, location_id, location_type, display_order, comment, created_at, created_by)
VALUES (esja_ultra_race_id, reykjavik_id, 'unknown', 1, 'Race located on Mt. Esja, near Reykjavík.', NOW(), seed_created_by)
    ON CONFLICT (race_id, location_id) DO NOTHING;

-- Hvítasunnuhlaup Hauka (hvitasunnuhlaup_hauka_race_id)
-- Location: Reykjavík (assuming it's in the greater capital area)
INSERT INTO public.race_locations (race_id, location_id, location_type, display_order, comment, created_at, created_by)
VALUES (hvitasunnuhlaup_hauka_race_id, reykjavik_id, 'unknown', 1, 'Race takes place in the greater Reykjavík area.', NOW(), seed_created_by)
    ON CONFLICT (race_id, location_id) DO NOTHING;

-- Ice Ultra (ice_ultra_id)
-- Location: Iceland (general location for a multi-stage race)
INSERT INTO public.race_locations (race_id, location_id, location_type, display_order, comment, created_at, created_by)
VALUES (ice_ultra_id, island_id, 'unknown', 1, 'Multi-stage race across various parts of Iceland.', NOW(), seed_created_by)
    ON CONFLICT (race_id, location_id) DO NOTHING;


---
--- Insert sample trail_locations connections
---

-- Laugavegur Trail (laugavegur_trail_id)
-- Start: Landmannalaugar
-- End: Þórsmörk
INSERT INTO public.trail_locations (trail_id, location_id, location_type, display_order, comment, created_at, created_by)
VALUES (laugavegur_trail_id, landmannalaugar_id, 'start', 1, 'Northern trailhead of Laugavegur.', NOW(), seed_created_by)
    ON CONFLICT (trail_id, location_id) DO NOTHING;

INSERT INTO public.trail_locations (trail_id, location_id, location_type, display_order, comment, created_at, created_by)
VALUES (laugavegur_trail_id, thorsmork_id, 'end', 2, 'Southern trailhead of Laugavegur.', NOW(), seed_created_by)
    ON CONFLICT (trail_id, location_id) DO NOTHING;

-- Hengill Ultra 52 (hengill_ultra_52_trail_id)
-- Location: Suðurland (Hveragerði area)
INSERT INTO public.trail_locations (trail_id, location_id, location_type, display_order, comment, created_at, created_by)
VALUES (hengill_ultra_52_trail_id, sudurland_id, 'unknown', 1, 'Trail primarily located within the Hengill geothermal area.', NOW(), seed_created_by)
    ON CONFLICT (trail_id, location_id) DO NOTHING;

-- Mt Esja Ultra marathon & half marathon trails (esja_ultra_marathon_trail_id, esja_ultra_half_marathon_trail_id)
-- Location: Reykjavík
INSERT INTO public.trail_locations (trail_id, location_id, location_type, display_order, comment, created_at, created_by)
VALUES (esja_ultra_marathon_trail_id, reykjavik_id, 'unknown', 1, 'Trail on Mt. Esja, easily accessible from Reykjavík.', NOW(), seed_created_by)
    ON CONFLICT (trail_id, location_id) DO NOTHING;

INSERT INTO public.trail_locations (trail_id, location_id, location_type, display_order, comment, created_at, created_by)
VALUES (esja_ultra_half_marathon_trail_id, reykjavik_id, 'unknown', 1, 'Trail on Mt. Esja, easily accessible from Reykjavík.', NOW(), seed_created_by)
    ON CONFLICT (trail_id, location_id) DO NOTHING;

-- Hvítasunnuhlaup Hauka trails (22km, 17km, 14km)
-- Location: Reykjavík (or more specifically, Hafnarfjörður if you had that location)
INSERT INTO public.trail_locations (trail_id, location_id, location_type, display_order, comment, created_at, created_by)
VALUES (hvitasunnuhlaup_hauka_22_trail_id, reykjavik_id, 'unknown', 1, 'Trail in the capital region.', NOW(), seed_created_by)
    ON CONFLICT (trail_id, location_id) DO NOTHING;

INSERT INTO public.trail_locations (trail_id, location_id, location_type, display_order, comment, created_at, created_by)
VALUES (hvitasunnuhlaup_hauka_17_trail_id, reykjavik_id, 'unknown', 1, 'Trail in the capital region.', NOW(), seed_created_by)
    ON CONFLICT (trail_id, location_id) DO NOTHING;

INSERT INTO public.trail_locations (trail_id, location_id, location_type, display_order, comment, created_at, created_by)
VALUES (hvitasunnuhlaup_hauka_14_trail_id, reykjavik_id, 'unknown', 1, 'Trail in the capital region.', NOW(), seed_created_by)
    ON CONFLICT (trail_id, location_id) DO NOTHING;

-- Bakgarður Náttúruhlaupa við Elliðavatn
-- Location: Reykjavík
INSERT INTO public.trail_locations (trail_id, location_id, location_type, display_order, comment, created_at, created_by)
VALUES (bakgardur_ellidavatn_trail_id, reykjavik_id, 'unknown', 1, 'Trail around Elliðavatn lake.', NOW(), seed_created_by)
    ON CONFLICT (trail_id, location_id) DO NOTHING;

-- Skaftafell Ultra Trail
-- Location: Suðurland (or a more specific Skaftafell location if you added one)
INSERT INTO public.trail_locations (trail_id, location_id, location_type, display_order, comment, created_at, created_by)
VALUES (skaftafell_ultra_trail_id, sudurland_id, 'unknown', 1, 'Trail within Skaftafell Nature Reserve.', NOW(), seed_created_by)
    ON CONFLICT (trail_id, location_id) DO NOTHING;

-- Puffin Run (puffin_run_trail_id)
-- Location: Vestmannaeyjar (if you add a location for it, otherwise default to Iceland or nearest region)
-- For now, let's connect it to Island if you don't have a specific Vestmannaeyjar location.
INSERT INTO public.trail_locations (trail_id, location_id, location_type, display_order, comment, created_at, created_by)
VALUES (puffin_run_trail_id, island_id, 'unknown', 1, 'Trail on Vestmannaeyjar (Westman Islands).', NOW(), seed_created_by)
    ON CONFLICT (trail_id, location_id) DO NOTHING;

END $$;