import { Container, Avatar, Text, Title, Paper, Group, Divider, Timeline } from '@mantine/core';

import {
    //IconClockHour4,
    IconMapPin, // For locations
    //IconTrekking, // For general outdoors/trekking
    IconBarcode, // For QR code scanning
    //IconRun, // For running
    IconAward, // For competition/awards
    IconRocket, // For launch!
    IconShare // For social media sharing
} from '@tabler/icons-react';
import {useEffect} from "react";

function AboutUsPage() {
    const updates = [
        {
            date: 'July 25, 2025',
            description: 'Hlaupaleiðir fer í loftið :D Víííí...',
            icon: <IconRocket size={18} />, // Changed to a rocket for "going live"
        },
        {
            date: 'July 26, 2025',
            description: 'Staðsetningar leiða komnar á vefinn',
            icon: <IconMapPin size={18} />, // Changed to a map pin for locations
        },
        {
            date: 'July 27, 2025',
            description: 'Upplýsingar um keppni eru með',
            icon: <IconAward size={18} />, // Changed to an award for competitions
        },
        {
            date: 'July 28, 2025',
            description: 'Skanna hlaupaleið í síma með QR kóða',
            icon: <IconBarcode size={18} />, // Changed to a barcode for QR code scanning
        },
        // Add more updates as needed with relevant icons
    ];

    // Sort updates: Newest first (descending order by date)
    const sortedUpdates = [...updates].sort((a, b) =>
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
                        src="https://images.unsplash.com/photo-1535713875002-d1d0cfd176db?crop=entropy&cs=tinysrgb&fit=max&fm=jpg&ixid=M3w0NTIyMDF8MHwxfHNlYXJjaHwxfHxwcm9maWxlJTIwcGljdHVyZXxlbnwwfHx8fDE3MjE2NzcwOTl8MA&ixlib=rb-4.0.3&q=80&w=400"
                        alt="Company Avatar"
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
                    Að hlauparar og útivistarfólk hafi þennan stað til að finna og deila skemmtilegum hlaupaleiðum út um allt land. Sjáumst á hlaupum :D
                </Text>

                <Divider my="xl" />

                <Title order={2} ta="center" mb="lg">
                    Uppfærslur
                </Title>
                <Timeline active={sortedUpdates.length} bulletSize={24} lineWidth={2}>
                    {/* "Coming Up" / Next Update Section */}
                    <Timeline.Item
                        bullet={<IconShare size={18} />} // Changed to IconShare for social media
                        title={<Text c="dimmed" fs="italic">Næst á dagskrá...</Text>}
                        lineVariant="dotted"
                        color="gray"
                        p="md"
                    >
                        <Text c="dimmed" size="sm" fs="italic">
                            Deila hlaupaleiðum á samfélagsmiðla
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