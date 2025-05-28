const express = require('express');
const axios = require('axios');
const cors = require('cors');
const dotenv = require('dotenv');
const path = require('path');

dotenv.config();

const app = express();
const PORT = process.env.PORT || 3000;
const NET_API_URL = process.env.BASE_URL;

app.use(express.json());
app.use(cors());

// ✅ Serve CSS and images manually
app.use('/css', express.static(path.join(__dirname, 'css')));
app.use('/images', express.static(path.join(__dirname, 'images')));

// ✅ Serve HTML
app.get('/products.html', (req, res) => {
    res.sendFile(path.join(__dirname, 'products.html'));
});
app.get('/index.html', (req, res) => {
    res.sendFile(path.join(__dirname, 'index.html'));
});

app.get('/about.html', (req, res) => {
    res.sendFile(path.join(__dirname, 'about.html'));
});

// ✅ Basic homepage
app.get('/', (req, res) => {
    res.send('<h1>Welcome to Kortbutiken</h1><p>Go to <a href="/products.html">Produkter</a></p>');
});

// ✅ API Routes (unchanged)
app.get('/api/cards', async (req, res) => {
    try {
        const response = await axios.get(`${NET_API_URL}/cards`);
        res.json(response.data);
    } catch (error) {
        res.status(500).json({ error: "Failed to fetch Cards" });
    }
});

app.listen(PORT, () => {
    console.log(`Server running at http://localhost:${PORT}`);
});
