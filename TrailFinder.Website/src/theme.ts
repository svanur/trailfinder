import type {MantineThemeOverride} from '@mantine/core';

export const theme: MantineThemeOverride = {
    fontFamily: 'Inter, Open Sans, sans-serif',
    headings: {
        fontFamily: 'Montserrat, sans-serif',
    },
    colors: {
        brand: [
            '#E6F0EC', '#CCE1D9', '#99C3B3', '#66A68C', '#338866',
            '#2E5E4E', '#264B3E', '#1D382F', '#152620', '#0A1310',
        ],
        accent: [
            '#FFF4E6', '#FFE8CC', '#FFD099', '#FFB866', '#FFA033',
            '#F28C28', '#C26F20', '#915218', '#61360F', '#301B07',
        ],
    },
    primaryColor: 'brand',
    primaryShade: 5,
    defaultRadius: 'md',

    /** Stílar sem mýkja yfirborð í ljósu þema **/
    components: {
        Button: {
            styles: () => ({ root: { fontWeight: 600 } }),
        },
        Card: {
            styles: (theme: { white: any; }) => ({
                root: {
                    backgroundColor: theme.white,
                    boxShadow: '0 2px 8px rgba(0,0,0,0.06)',
                },
            }),
        },
        Menu: {
            styles: (theme: { white: any; colors: { gray: any[]; }; }) => ({
                dropdown: {
                    backgroundColor: theme.white,
                    boxShadow: '0 2px 8px rgba(0,0,0,0.08)',
                },
                item: {
                    '&[data-hovered]': {
                        backgroundColor: theme.colors.gray[0],
                    },
                },
            }),
        },
    },
};
