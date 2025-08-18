const Footer = () => {
    return (
        <footer className="bg-gray-100 mt-8">
            <div className="container mx-auto px-4 py-6">
                <p className="text-center text-gray-600">
                    © {new Date().getFullYear()} Hlaupaleiðir. Allur réttur áskilinn.
                </p>
            </div>
        </footer>
    );
};

export default Footer;