import React from 'react';
import { Document, Page, Text, View, Image, StyleSheet } from '@react-pdf/renderer';
import { IRecipe } from '../interfaces/RecipeInterface';

const quantityTypes = [
  { value: 1, label: 'None' },
  { value: 2, label: 'Grams' },
  { value: 3, label: 'Kilograms' },
  { value: 4, label: 'Milliliters' },
  { value: 5, label: 'Liters' },
  { value: 6, label: 'Pieces' },
  { value: 7, label: 'Slices' },
  { value: 8, label: 'Cups' },
  { value: 9, label: 'Teaspoons' },
  { value: 10, label: 'Tablespoons' },
];

const getQuantityTypeText = (quantityType: number): string => {
  const type = quantityTypes.find((qt) => qt.value === quantityType);
  return type ? type.label : '';
};

const styles = StyleSheet.create({
  page: {
    padding: 20
  },
  section: {
    marginBottom: 10
  },
  image: {
    width: '100%',
    height: 'auto'
  },
  title: {
    fontSize: 24,
    marginBottom: 10
  },
  text: {
    fontSize: 12,
    marginBottom: 5
  },
});

interface RecipePdfDocumentProps {
  recipe: IRecipe;
}

const RecipePdfDocument: React.FC<RecipePdfDocumentProps> = ({ recipe }) => (
  <Document>
    <Page style={styles.page}>
      <View style={styles.section}>
        <Text style={styles.title}>{recipe.name}</Text>
      </View>
      <View style={styles.section}>
        <Text style={styles.text}>Estimated Time: {recipe.estimatedTime}</Text>
      </View>
      <View style={styles.section}>
        <Text style={styles.title}>Ingredients</Text>
        {recipe.recipeIngredients.map((ingredient, index) => (
          <Text key={index} style={styles.text}>
            {ingredient.quantity} x {ingredient.quantityType !== 1 && `${getQuantityTypeText(ingredient.quantityType)} `}
            {ingredient.name}
          </Text>
        ))}
      </View>
      <View style={styles.section}>
        <Text style={styles.title}>Description</Text>
        <Text style={styles.text}>{recipe.description}</Text>
      </View>
    </Page>
  </Document>
);

export default RecipePdfDocument;
