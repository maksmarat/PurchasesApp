import { useState } from 'react';
import { useEffect } from 'react';

function App() {
  const [productName, setProductName] = useState('');
  const [productPrice, setProductPrice] = useState(0);
  const [purchases, setPurchases] = useState([]);

  useEffect(() => {
    loadPurchases();
  }, []);

  const loadPurchases = async () => {
    const response = await fetch("http://localhost:5000/purchases");
    const data = await response.json();
    setPurchases(data);
  };

  const addProduct = async () => {
    if (productName == "") { return; }
    if (Number(productPrice) == 0) { return; }

    await fetch("http://localhost:5000/purchases", {
      method: "POST",
      headers: {
        "Content-Type": "application/json"
      },
      body: JSON.stringify({
        name: productName,
        price: Number(productPrice)
      })
    });

    setProductName("");
    setProductPrice("");

    loadPurchases();
  };

  const deleteProducts = async () => {
    await fetch("http://localhost:5000/purchases", {
      method: "DELETE"
    });

    loadPurchases();
  };

  const productNameChange = (e) => {
    setProductName(e.target.value);
  };

  const productPriceChange = (e) => {
    setProductPrice(e.target.value);
  };

  return (
    <div>
      <h1>Мои покупки</h1>
      <input 
        type="text"
        placeholder="Название"
        value={productName}
        onChange={productNameChange}
      />
      <input 
        type="number"
        placeholder="Цена"
        value={productPrice}
        onChange={productPriceChange}
      />
      <button onClick={addProduct}>Добавить</button>
      <button onClick={deleteProducts}>Удалить продукты</button>

      {purchases.map((item) => (
        <div key={item.id}>
          {item.name} - {item.price} RSD {item.time}
        </div>
      ))}
    </div>
  );
}

export default App;
