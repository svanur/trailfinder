import { Container, Avatar, Text, Title, Paper, Group, Divider, Timeline } from '@mantine/core';

import {
    //IconBarcode, // For QR code scanning
    IconGitPullRequest, // For new updates
    IconRocket, // For launch!
    //IconShare // For social media sharing
    //IconClockHour4,
    //IconMapPin, // For locations
    //IconTrekking, // For general outdoors/trekking
    
    //IconRun, // For running
    //IconAward, // For competition/awards
} from '@tabler/icons-react';
import {useEffect} from "react";

function AboutUsPage() {
    const nextUpdate =
        {
            description: 'Hlaupa Laugaveginn á sub 4 #teamSub4',
            icon: <IconGitPullRequest size={18} />
        };
    
    const updateHistory = [
        {
            date: 'July 25, 2025',
            description: 'Hlaupaleiðir fer í loftið :D #GAMAN',
            icon: <IconRocket size={18} />, // a rocket for "going live"
        },

        // Add more updateHistory as needed with relevant icons
        /*
        {
            date: 'July 26, 2025',
            description: 'Skanna hlaupaleið í síma með QR kóða',
            icon: <IconBarcode size={18} />
        },
        {
            date: 'July 26, 2025',
            description: 'Staðsetningar leiða komnar á vefinn',
            icon: <IconMapPin size={18} />, // a map pin for locations
        },
        {
            date: 'July 27, 2025',
            description: 'Upplýsingar um keppni eru með',
            icon: <IconAward size={18} />, // an award for competitions
        },
        */
    ];

    // Sort updateHistory: Newest first (descending order by date)
    const sortedUpdates = [...updateHistory].sort((a, b) =>
        new Date(b.date).getTime() - new Date(a.date).getTime()
    );

    // Use useEffect to run the obfuscation code after the component mounts
    useEffect(() => {
        const user = "svanur.karlsson";
        const domain = "gmail";
        const tld = "com";
        const emailLinkElement = document.getElementById("emailLink");

        if (emailLinkElement) { // Check if the element exists before manipulating it
            emailLinkElement.setAttribute("href", `mailto:${user}@${domain}.${tld}`);
            emailLinkElement.textContent = `${user} [hjá] ${domain} [punktur] ${tld}`;
        }
    }, []); // Empty dependency array means this runs once after the initial render


    return (
        <Container size="md" my="xl">
            <Paper shadow="sm" p="xl" withBorder>
                <Group align="center" gap="lg">
                    <Avatar
                        src="/um.jpg"
                        alt="Svanur Karlsson"
                        size={120}
                        radius="50%"
                    />
                    <Title order={1} ta="center">
                        hlaupaleidir.is
                    </Title>
                    <Text size="lg" ta="center">
                        Velkomin á Hlaupaleiðir.is. Hér sameinast nördisminn minn sem forritari og náttúruhlaupari. Ég hef í nokkurn tíma safnað áhugaverðum hlaupaleiðum sem mig langar að fara 'einn góðan veðurdag'. En ...svo er bara svo mikið annað að gera!
                        En, vonandi kemst maður eitthvað af öllum þessum hlaupaleiðum í framtíðinni.
                        Vonandi hafið þið gagn og gaman af :)
                    </Text>
                </Group>

                <Divider my="xl" />

                <Title order={2} ta="center" mb="lg">
                    Markmiðið
                </Title>
                <Text ta="center">
                    Að hlauparar og útivistarfólk hafi þennan stað til að finna og deila skemmtilegum hlaupaleiðum út um allt land. Sjáumst á hlaupum.
                </Text>

                <Divider my="xl" />

                <Title order={2} ta="center" mb="lg">
                    Uppfærslur
                </Title>
                <Timeline active={sortedUpdates.length} bulletSize={24} lineWidth={2}>
                    {/* "Coming Up" / Next Update Section */}
                    <Timeline.Item
                        bullet={nextUpdate.icon}
                        title={<Text c="dimmed" fs="italic">Næsta viðbót</Text>}
                        lineVariant="dotted"
                        color="gray"
                        p="md"
                    >
                        <Text c="dimmed" size="sm" fs="italic">
                            {nextUpdate.description}
                        </Text>
                    </Timeline.Item>

                    {/* Sorted Updates (Newest on top) */}
                    {sortedUpdates.map((update, index) => (
                        <Timeline.Item
                            key={index}
                            bullet={update.icon}
                            title={update.date}
                        >
                            <Text c="dimmed" size="sm">
                                {update.description}
                            </Text>
                        </Timeline.Item>
                    ))}
                </Timeline>

                <Divider my="xl" />

                <Title c="dimmed" order={2} ta="center" mb="lg">
                    Ert þú með hugmynd?
                </Title>
                <Text c="dimmed" ta="center">
                    Sendu mér þína frábæru hugmynd fyrir vefinn á netfangið {' '}
                    <a id="emailLink" style={{ textDecoration: 'none', color: 'inherit' }}>

                    </a>
                </Text>
                <Text c="dimmed" ta="center">...og hver veit nema hún komist einn daginn á uppfærslulistann :)</Text>

            </Paper>
        </Container>
    );
}

export default AboutUsPage;