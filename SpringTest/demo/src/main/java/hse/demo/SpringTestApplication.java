package hse.demo;

import java.util.ArrayList;
import java.util.ListIterator;

import org.springframework.boot.SpringApplication;
import org.springframework.boot.autoconfigure.SpringBootApplication;
import org.springframework.web.bind.annotation.DeleteMapping;
import org.springframework.web.bind.annotation.GetMapping;
import org.springframework.web.bind.annotation.PathVariable;
import org.springframework.web.bind.annotation.PostMapping;
import org.springframework.web.bind.annotation.PutMapping;
import org.springframework.web.bind.annotation.RequestBody;
import org.springframework.web.bind.annotation.RestController;

@SpringBootApplication
@RestController
public class SpringTestApplication {
	private String secret = "none";
	private ArrayList<Item> items = new ArrayList<>();

	public static void main(String[] args) {
		SpringApplication.run(SpringTestApplication.class, args);
	}

	@GetMapping("/hello")
	public String getHello() {
		return "Hello World";
	}

	@GetMapping("/echo/{value}")
	public String echo(@PathVariable String value) {
		return value;
	}

	@PostMapping("/secret")
	public String setSecret(@RequestBody String newSecret) {
		secret = newSecret;
		return secret;
	}

	@GetMapping("/secret")
	public String getSecret() {
		return secret;
	}

	@DeleteMapping("/secret")
	public void resetSecret() {
		secret = "none";
	}

	@PutMapping("/secret")
	public String appendSecret(@RequestBody String part) {
		secret += part;
		return secret;
	}

	@PostMapping(path = "/item", consumes = "application/json", produces = "application/json")
	public Item createItem(@RequestBody Item item) {
		// don't add duplicates
		for (Item i : items) {
			if (i.getName().equals(item.getName())) {
				return new Item ("already exists", i.getAmount());
			}
		}

		items.add(item);
		return item;
	}

	@GetMapping(path = "/item", produces = "application/json")
	public ArrayList<Item> getItems() {
		return items;
	}

	@DeleteMapping(path = "/item")
	public String deleteItem(@RequestBody String name) {
		if(items.removeIf(item -> item.getName().equals(name))) {
			return "success";
		}
		else {
			return "item could not be found";
		}
	}

	@PutMapping(path = "/item", consumes = "application/json", produces = "application/json")
	public Item updateItem(@RequestBody Item item) {
		for (Item i : items) {
			if (i.getName().equals(item.getName())) {
				i.setAmount(item.getAmount());
				return i;
			}
		}
		return new Item("item not found", 0);
	}

}
