import { useState } from 'react';

function App() {
  const [productName, setProductName] = useState('');
  const [productPrice, setProductPrice] = useState(0);

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
    </div>
  );
}

export default App;